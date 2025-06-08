using UnityEngine; // Necesario para scripts de Unity
using TMPro; // Para usar componentes TextMeshPro
using UnityEngine.UI; // Para componentes UI (botones, etc.)
using UnityEngine.Networking; // Para hacer peticiones HTTP
using System.Collections; // Para usar corrutinas
using System.Text; // Para codificar el JSON en bytes
using Assets.Scripts; // Para usar DTOs como RegistroRequest

// Clase que gestiona el registro y login de usuarios en la interfaz de Unity
public class AuthManager : MonoBehaviour
{
    // Campos de entrada y feedback para el panel de registro
    public TMP_InputField inputNombre;
    public TMP_InputField inputEmailRegistro;
    public TMP_InputField inputContraseñaRegistro;
    public TextMeshProUGUI textoFeedbackRegistro;

    // Campos de entrada y feedback para el panel de login
    public TMP_InputField inputEmailLogin;
    public TMP_InputField inputContraseñaLogin;
    public TextMeshProUGUI textoFeedbackLogin;

    // Referencias a los paneles para alternar entre login y registro
    public GameObject panelRegistro;
    public GameObject panelLogin;

    // URL base de la API
    private string apiUrl = "http://localhost:5195/api/auth";

    // Al iniciar el script, se muestra el panel de login
    void Start()
    {
        MostrarPanelLogin(); // Iniciar con el panel de login
    }

    // Cambia a la vista del panel de registro
    public void MostrarPanelRegistro()
    {
        panelLogin.SetActive(false);
        panelRegistro.SetActive(true);
    }

    // Cambia a la vista del panel de login
    public void MostrarPanelLogin()
    {
        panelRegistro.SetActive(false);
        panelLogin.SetActive(true);
    }

    // Valida los datos del formulario de registro antes de enviarlos
    public void ValidarRegistro()
    {
        if (string.IsNullOrEmpty(inputNombre.text))
        {
            textoFeedbackRegistro.text = "¡El nombre es obligatorio!";
            return;
        }
        if (string.IsNullOrEmpty(inputEmailRegistro.text))
        {
            textoFeedbackRegistro.text = "¡El email es obligatorio!";
            return;
        }
        if (!EsEmailValido(inputEmailRegistro.text))
        {
            textoFeedbackRegistro.text = "¡Formato de email inválido!";
            return;
        }

        if (string.IsNullOrEmpty(inputContraseñaRegistro.text))
        {
            textoFeedbackRegistro.text = "¡La contraseña es obligatoria!";
            return;
        }
        if (inputContraseñaRegistro.text.Length < 6)
        {
            textoFeedbackRegistro.text = "¡La contraseña debe tener al menos 6 caracteres!";
            return;
        }

        StartCoroutine(RegistrarUsuario()); // Si todo es válido, empieza la corrutina
    }

    // Función auxiliar para verificar el formato del email
    private bool EsEmailValido(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Corrutina que envía la petición de registro al backend
    IEnumerator RegistrarUsuario()
    {
        var datosRegistro = new RegistroRequest
        {
            nombre = inputNombre.text,
            email = inputEmailRegistro.text,
            contraseña = inputContraseñaRegistro.text
        };

        string jsonData = JsonUtility.ToJson(datosRegistro); // Convierte el objeto a JSON

        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/registro", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest(); // Espera la respuesta del servidor

            if (www.result == UnityWebRequest.Result.Success || www.responseCode == 200 || www.responseCode == 201)
            {
                textoFeedbackRegistro.text = "¡Registro exitoso!";
                MostrarPanelLogin(); // Vuelve al login
            }
            else if (www.responseCode == 409)
            {
                var error = JsonUtility.FromJson<ErrorResponse>(www.downloadHandler.text);
                textoFeedbackRegistro.text = "❌ " + error.mensaje;
            }
            else
            {
                textoFeedbackRegistro.text = "❌ Error al registrar: " + www.error;
            }
        }
    }

    // Valida los campos del login antes de enviarlos
    public void ValidarLogin()
    {
        if (string.IsNullOrEmpty(inputEmailLogin.text))
        {
            textoFeedbackLogin.text = "¡El email es obligatorio!";
            return;
        }
        if (!EsEmailValido(inputEmailLogin.text))
        {
            textoFeedbackLogin.text = "¡Formato de email inválido!";
            return;
        }

        if (string.IsNullOrEmpty(inputContraseñaLogin.text))
        {
            textoFeedbackLogin.text = "¡La contraseña es obligatoria!";
            return;
        }
        if (inputContraseñaLogin.text.Length < 6)
        {
            textoFeedbackLogin.text = "¡Contraseña demasiado corta!";
            return;
        }

        StartCoroutine(IniciarSesion()); // Si es válido, lanza la corrutina
    }

    // Corrutina que envía la solicitud de login
    IEnumerator IniciarSesion()
    {
        var datosLogin = new LoginRequest
        {
            email = inputEmailLogin.text,
            contraseña = inputContraseñaLogin.text
        };

        string jsonData = JsonUtility.ToJson(datosLogin); // Convierte el objeto a JSON

        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest(); // Espera la respuesta

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Respuesta JSON: " + www.downloadHandler.text); // Para depuración

                LoginResponse respuesta = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

                // Guardar los datos del usuario en PlayerPrefs
                PlayerPrefs.SetString("token", respuesta.token);
                PlayerPrefs.SetString("nombre", respuesta.nombre);
                PlayerPrefs.SetString("rol", respuesta.rol);
                PlayerPrefs.SetString("email", respuesta.email);
                Debug.Log("Email guardado en PlayerPrefs: " + PlayerPrefs.GetString("email"));

                textoFeedbackLogin.text = "¡Bienvenido, " + respuesta.nombre + "!";
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene"); // Cargar escena principal
            }
            else if (www.responseCode == 401)
            {
                var error = JsonUtility.FromJson<ErrorResponse>(www.downloadHandler.text);
                textoFeedbackLogin.text = "❌ " + error.mensaje;
            }
            else
            {
                textoFeedbackLogin.text = "❌ Error de conexión o inesperado: " + www.error;
            }
        }
    }
}
