using UnityEngine; // Funcionalidades básicas de Unity
using TMPro; // Para manejar campos de texto y feedback en UI
using UnityEngine.UI; // Para trabajar con elementos UI (aunque no se usa directamente aquí)
using UnityEngine.Networking; // Para hacer peticiones HTTP
using System.Collections; // Para usar corrutinas
using System.Text; // Para convertir strings en bytes
using Assets.Scripts; // Para usar EditarUsuarioRequest

// Clase encargada de gestionar la edición de la cuenta del usuario
public class GestionCuentaManager : MonoBehaviour
{
    public TMP_InputField inputNuevoNombre; // Campo de texto para nuevo nombre
    public TMP_InputField inputNuevaContraseña; // Campo de texto para nueva contraseña
    public TextMeshProUGUI textoFeedback; // Texto para mostrar mensajes de validación o éxito

    private string apiUrl = "http://localhost:5195/api/auth"; // URL base de la API de autenticación

    // Método que se llama al pulsar el botón "Guardar Cambios"
    public void EditarCuenta()
    {
        string nuevoNombre = inputNuevoNombre.text.Trim(); // Obtener y limpiar el nombre
        string nuevaContraseña = inputNuevaContraseña.text.Trim(); // Obtener y limpiar la contraseña

        // Validaciones básicas
        if (string.IsNullOrEmpty(nuevoNombre))
        {
            textoFeedback.text = "⚠️ El nombre no puede estar vacío.";
            return;
        }

        if (string.IsNullOrEmpty(nuevaContraseña))
        {
            textoFeedback.text = "⚠️ La contraseña no puede estar vacía.";
            return;
        }

        if (nuevaContraseña.Length < 6)
        {
            textoFeedback.text = "⚠️ La contraseña debe tener al menos 6 caracteres.";
            return;
        }

        // Obtener el email del usuario desde PlayerPrefs
        string email = PlayerPrefs.GetString("email", "");
        Debug.Log("Email leído en edición: " + email);

        // Validar que se ha obtenido correctamente
        if (string.IsNullOrEmpty(email))
        {
            textoFeedback.text = "⚠️ No se encontró el email del usuario.";
            return;
        }

        // Iniciar la corrutina que envía los datos a la API
        StartCoroutine(EnviarEdicion(email));
    }

    // Corrutina que envía la solicitud PUT para editar el usuario
    IEnumerator EnviarEdicion(string email)
    {
        // Crear el objeto con los nuevos datos
        var datos = new EditarUsuarioRequest
        {
            email = email,
            nuevoNombre = inputNuevoNombre.text,
            nuevaContraseña = inputNuevaContraseña.text
        };

        // Convertir los datos a JSON
        string json = JsonUtility.ToJson(datos);

        // Crear la solicitud HTTP PUT
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/editar", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json); // Codificar el JSON
            www.uploadHandler = new UploadHandlerRaw(bodyRaw); // Enviar el cuerpo
            www.downloadHandler = new DownloadHandlerBuffer(); // Leer respuesta
            www.SetRequestHeader("Content-Type", "application/json"); // Tipo de contenido

            // Esperar la respuesta
            yield return www.SendWebRequest();

            // Si todo ha ido bien
            if (www.result == UnityWebRequest.Result.Success)
            {
                textoFeedback.text = "¡Cuenta actualizada!";

                // Si cambió el nombre, lo guardamos en PlayerPrefs
                if (!string.IsNullOrEmpty(datos.nuevoNombre))
                {
                    PlayerPrefs.SetString("nombre", datos.nuevoNombre);

                    // Actualizar el nombre en el menú si está presente
                    MenuManager menu = FindObjectOfType<MenuManager>();
                    if (menu != null)
                    {
                        menu.ActualizarNombreDesdePrefs();
                    }
                }
            }
            // Si el servidor responde con conflicto (nombre ya usado)
            else if (www.responseCode == 409)
            {
                ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(www.downloadHandler.text);
                textoFeedback.text = "❌ " + error.mensaje;
            }
            // Otro tipo de error
            else
            {
                textoFeedback.text = "❌ Error al editar: " + www.error;
                Debug.Log("Respuesta completa: " + www.downloadHandler.text);
            }
        }
    }
}
