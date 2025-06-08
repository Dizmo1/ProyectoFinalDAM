using UnityEngine; // Funcionalidades principales de Unity.
using UnityEngine.Networking; // Para realizar peticiones HTTP.
using UnityEngine.Events; // Para usar UnityEvent.
using UnityEngine.SceneManagement; // Para cambiar de escena.
using System.Collections; // Para corrutinas.
using System.Text; // Para codificar strings.
using System; // Funcionalidades básicas (excepciones, tipos primitivos).
using Assets.Scripts.DTO; // Para acceder a clases DTO (como PartidaResponse y TiroData).
using Assets.Scripts; // Para otros scripts del proyecto.

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton para acceso global al GameManager.

    [Header("API Configuration")]
    [SerializeField] private string apiUrl = "http://localhost:5195/api"; // URL base de la API.
    [SerializeField] private float requestTimeout = 10f; // Tiempo máximo de espera en una petición.

    [Header("Events")]
    public UnityEvent<int> OnPartidaCreada; // Evento para notificar que una partida fue creada.
    public UnityEvent<bool, float> OnTiroRegistrado; // Evento para notificar un tiro registrado.
    public UnityEvent<string> OnError; // Evento para notificar errores.

    private int partidaActualId = -1; // ID de la partida actual (-1 indica que no hay partida).
    private string jwtToken; // Token JWT guardado.
    private float tiempoInicioPartida; // Momento en que se inició la partida.
    private int puntosTotales = 0; // Puntos acumulados durante la partida.

    private void Awake()
    {
        // Control del patrón Singleton para que solo exista una instancia de GameManager.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantener GameManager entre escenas.
        jwtToken = PlayerPrefs.GetString("token"); // Leer token almacenado.
        Debug.Log("🎫 Token leído en GameManager: " + jwtToken);
    }

    // 👉 Llamado al pulsar botón "Jugar"
    public void IniciarNuevaPartida()
    {
        // Validar que hay token (usuario autenticado)
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogWarning("❌ Usuario no autenticado. No se puede crear partida.");
            OnError?.Invoke("Usuario no autenticado");
            return;
        }

        Debug.Log("📌 GameManager está activo. Iniciando nueva partida...");
        StartCoroutine(IniciarPartidaCoroutine());
    }

    public void SumarPuntos(int cantidad)
    {
        // Incrementar puntos totales
        puntosTotales += cantidad;
    }

    public void FinalizarPartida()
    {
        if (partidaActualId == -1)
        {
            Debug.LogWarning("⚠️ No hay partida activa para finalizar.");
            return;
        }

        Debug.Log($"✅ Partida finalizada: ID = {partidaActualId}");

        ResumenPartidaManager resumen = FindObjectOfType<ResumenPartidaManager>();
        if (resumen != null)
        {
            string nombre = PlayerPrefs.GetString("nombre", "Jugador");
            float tiempo = Time.time - tiempoInicioPartida;
            int puntos = puntosTotales;

            resumen.MostrarResumen(nombre, puntos, tiempo);
        }
        else
        {
            Debug.LogWarning("❌ No se encontró ResumenPartidaManager. No se mostrará resumen.");
        }

        partidaActualId = -1;
    }


    private IEnumerator IniciarPartidaCoroutine()
    {
        // Corrutina que envía una solicitud POST para crear una nueva partida
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/Partidas/nuevaPartida", "POST"))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // Para leer respuesta
            SetupRequest(www); // Configurar cabeceras

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                HandlePartidaResponse(www); // Procesar respuesta
                SceneManager.LoadScene("GameScene"); // Cargar escena del juego
            }
            else
            {
                HandleError(www, "Error al crear partida");
            }
        }
    }

    private void HandlePartidaResponse(UnityWebRequest www)
    {
        try
        {
            // Interpretar JSON de la respuesta
            PartidaResponse response = JsonUtility.FromJson<PartidaResponse>(www.downloadHandler.text);
            partidaActualId = response.partidaId; // Guardar ID de la partida
            tiempoInicioPartida = Time.time; // Registrar momento de inicio

            Debug.Log($"✅ Partida iniciada. ID = {partidaActualId}");
            OnPartidaCreada?.Invoke(partidaActualId); // Lanzar evento
        }
        catch (Exception e)
        {
            OnError?.Invoke($"Error procesando respuesta: {e.Message}");
        }
    }

    public void RegistrarTiro(bool acierto, Vector3 posicionInicial, Vector3 posicionFinal)
    {
        // Validar partida activa
        if (partidaActualId == -1)
        {
            OnError?.Invoke("No hay partida activa");
            return;
        }

        float tiempoTranscurrido = Time.time - tiempoInicioPartida; // Tiempo del tiro
        float distancia = Vector3.Distance(posicionInicial, posicionFinal); // Distancia del tiro

        Debug.Log($"➡️ Enviando tiro: Acierto={acierto}, Tiempo={tiempoTranscurrido}, Distancia={distancia}");

        StartCoroutine(RegistrarTiroCoroutine(acierto, tiempoTranscurrido, distancia));
    }

    private IEnumerator RegistrarTiroCoroutine(bool acierto, float tiempo, float distancia)
    {
        // Preparar datos del tiro
        TiroData tiroData = new TiroData
        {
            partidaId = partidaActualId,
            acierto = acierto,
            tiempoSegundos = tiempo,
            distancia = distancia
        };

        // Enviar solicitud POST a la API
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/Partidas/registrarTiro", "POST"))
        {
            byte[] body = Encoding.UTF8.GetBytes(JsonUtility.ToJson(tiroData));
            www.uploadHandler = new UploadHandlerRaw(body);
            www.downloadHandler = new DownloadHandlerBuffer();
            SetupRequest(www);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Tiro registrado correctamente.");
                OnTiroRegistrado?.Invoke(acierto, distancia); // Lanzar evento
            }
            else
            {
                Debug.LogError($"❌ Error registrando tiro: {www.responseCode} - {www.error}");
                if (!string.IsNullOrEmpty(www.downloadHandler.text))
                    Debug.LogError($"Detalles: {www.downloadHandler.text}");
            }
        }
    }

    private void SetupRequest(UnityWebRequest www)
    {
        // Configurar cabeceras comunes para la API
        www.timeout = (int)requestTimeout;
        www.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");
    }

    private void HandleError(UnityWebRequest www, string mensajeBase)
    {
        // Manejo genérico de errores
        string errorMessage = $"{mensajeBase}: [{www.responseCode}] {www.error}";
        if (!string.IsNullOrEmpty(www.downloadHandler.text))
        {
            errorMessage += $"\nDetalles: {www.downloadHandler.text}";
        }
        Debug.LogError(errorMessage);
        OnError?.Invoke(errorMessage);
    }

    public bool HayPartidaActiva()
    {
        return partidaActualId != -1; // Verificar si hay partida en curso
    }

    public int ObtenerIdPartida()
    {
        return partidaActualId; // Obtener ID de partida activa
    }
}
