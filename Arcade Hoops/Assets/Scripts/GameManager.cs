using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using System;
using Assets.Scripts.DTO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("API Configuration")]
    [SerializeField] private string apiUrl = "http://localhost:5195/api"; // ← SIN /Partidas
    [SerializeField] private float requestTimeout = 10f;

    [Header("Events")]
    public UnityEvent<int> OnPartidaCreada;
    public UnityEvent<bool, float> OnTiroRegistrado;
    public UnityEvent<string> OnError;

    private int partidaActualId = -1;
    private string jwtToken;
    private float tiempoInicioPartida;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        jwtToken = PlayerPrefs.GetString("token");
        Debug.Log("🎫 Token leído en GameManager: " + jwtToken);

    }

    // 👉 Llamado al pulsar botón "Jugar"
    public void IniciarNuevaPartida()
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogWarning("❌ Usuario no autenticado. No se puede crear partida.");
            OnError?.Invoke("Usuario no autenticado");
            return;
        }

        Debug.Log("📌 GameManager está activo. Iniciando nueva partida...");
        StartCoroutine(IniciarPartidaCoroutine());
    }

    private IEnumerator IniciarPartidaCoroutine()
    {
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/Partidas/nuevaPartida", "POST"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            SetupRequest(www);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                HandlePartidaResponse(www);
                SceneManager.LoadScene("GameScene"); // 👈 Ir a la escena después de crear partida
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
            PartidaResponse response = JsonUtility.FromJson<PartidaResponse>(www.downloadHandler.text);
            partidaActualId = response.partidaId;
            tiempoInicioPartida = Time.time;

            Debug.Log($"✅ Partida iniciada. ID = {partidaActualId}");
            OnPartidaCreada?.Invoke(partidaActualId);
        }
        catch (Exception e)
        {
            OnError?.Invoke($"Error procesando respuesta: {e.Message}");
        }
    }

    public void RegistrarTiro(bool acierto, Vector3 posicionInicial, Vector3 posicionFinal)
    {
        if (partidaActualId == -1)
        {
            OnError?.Invoke("No hay partida activa");
            return;
        }

        float tiempoTranscurrido = Time.time - tiempoInicioPartida;
        float distancia = Vector3.Distance(posicionInicial, posicionFinal);

        Debug.Log($"➡️ Enviando tiro: Acierto={acierto}, Tiempo={tiempoTranscurrido}, Distancia={distancia}");

        StartCoroutine(RegistrarTiroCoroutine(acierto, tiempoTranscurrido, distancia));
    }

    private IEnumerator RegistrarTiroCoroutine(bool acierto, float tiempo, float distancia)
    {
        TiroData tiroData = new TiroData
        {
            partidaId = partidaActualId,
            acierto = acierto,
            tiempoSegundos = tiempo,
            distancia = distancia
        };

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
                OnTiroRegistrado?.Invoke(acierto, distancia);
            }
            else
            {
                Debug.LogError($"❌ Error registrando tiro: {www.responseCode} - {www.error}");
                if (!string.IsNullOrEmpty(www.downloadHandler.text))
                    Debug.LogError($"Detalles: {www.downloadHandler.text}");
            }
        }
    }

    public void FinalizarPartida()
    {
        if (partidaActualId == -1)
        {
            Debug.LogWarning("⚠️ No hay partida activa para finalizar.");
            return;
        }

        Debug.Log($"✅ Partida finalizada: ID = {partidaActualId}");
        partidaActualId = -1;
    }

    private void SetupRequest(UnityWebRequest www)
    {
        www.timeout = (int)requestTimeout;
        www.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");
    }

    private void HandleError(UnityWebRequest www, string mensajeBase)
    {
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
        return partidaActualId != -1;
    }

    public int ObtenerIdPartida()
    {
        return partidaActualId;
    }
}
