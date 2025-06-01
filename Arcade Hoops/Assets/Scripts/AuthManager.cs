using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Assets.Scripts;

public class AuthManager : MonoBehaviour
{
    // Referencias UI
    public TMP_InputField inputNombre;
    public TMP_InputField inputEmailRegistro;
    public TMP_InputField inputContraseñaRegistro;
    public TextMeshProUGUI textoFeedbackRegistro;

    public TMP_InputField inputEmailLogin;
    public TMP_InputField inputContraseñaLogin;
    public TextMeshProUGUI textoFeedbackLogin;

    public GameObject panelRegistro;
    public GameObject panelLogin;

    private string apiUrl = "http://localhost:5195/api/auth";

    void Start()
    {
        MostrarPanelLogin(); // Iniciar con el panel de login
    }

    // Alternar paneles
    public void MostrarPanelRegistro()
    {
        panelLogin.SetActive(false);
        panelRegistro.SetActive(true);
    }

    public void MostrarPanelLogin()
    {
        panelRegistro.SetActive(false);
        panelLogin.SetActive(true);
    }

    // Lógica de Registro (Botón "Registrarse")
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

        StartCoroutine(RegistrarUsuario());
    }

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


    IEnumerator RegistrarUsuario()
    {
        var datosRegistro = new RegistroRequest
        {
            nombre = inputNombre.text,
            email = inputEmailRegistro.text,
            contraseña = inputContraseñaRegistro.text
        };

        string jsonData = JsonUtility.ToJson(datosRegistro);

        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/registro", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success || www.responseCode == 200 || www.responseCode == 201)
            {
                textoFeedbackRegistro.text = "¡Registro exitoso!";
                MostrarPanelLogin();
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


    // Lógica de Login (Botón "Iniciar Sesión")
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

        StartCoroutine(IniciarSesion());
    }

    IEnumerator IniciarSesion()
    {
        var datosLogin = new LoginRequest
        {
            email = inputEmailLogin.text,
            contraseña = inputContraseñaLogin.text
        };


        string jsonData = JsonUtility.ToJson(datosLogin);

        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Respuesta JSON: " + www.downloadHandler.text);  // <--- Añade esto para ver el JSON

                LoginResponse respuesta = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

                PlayerPrefs.SetString("token", respuesta.token);
                PlayerPrefs.SetString("nombre", respuesta.nombre);
                PlayerPrefs.SetString("rol", respuesta.rol);
                PlayerPrefs.SetString("email", respuesta.email);
                Debug.Log("Email guardado en PlayerPrefs: " + PlayerPrefs.GetString("email"));




                textoFeedbackLogin.text = "¡Bienvenido, " + respuesta.nombre + "!";
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
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