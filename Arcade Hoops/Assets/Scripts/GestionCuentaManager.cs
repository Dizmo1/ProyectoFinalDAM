using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Assets.Scripts;

public class GestionCuentaManager : MonoBehaviour
{
    public TMP_InputField inputNuevoNombre;
    public TMP_InputField inputNuevaContraseña;
    public TextMeshProUGUI textoFeedback;

    private string apiUrl = "http://localhost:5195/api/auth";

    public void EditarCuenta()
    {
        string nuevoNombre = inputNuevoNombre.text.Trim();
        string nuevaContraseña = inputNuevaContraseña.text.Trim();

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

        // Recuperar email del usuario logueado
        string email = PlayerPrefs.GetString("email", "");
        Debug.Log("Email leído en edición: " + email);

        if (string.IsNullOrEmpty(email))
        {
            textoFeedback.text = "⚠️ No se encontró el email del usuario.";
            return;
        }

        StartCoroutine(EnviarEdicion(email));
    }

    IEnumerator EnviarEdicion(string email)
    {
        var datos = new EditarUsuarioRequest
        {
            email = email,
            nuevoNombre = inputNuevoNombre.text,
            nuevaContraseña = inputNuevaContraseña.text
        };

        string json = JsonUtility.ToJson(datos);

        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/editar", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                textoFeedback.text = "¡Cuenta actualizada!";

                if (!string.IsNullOrEmpty(datos.nuevoNombre))
                {
                    PlayerPrefs.SetString("nombre", datos.nuevoNombre);

                    // 🔄 Actualizar el nombre en el menú si está activo
                    MenuManager menu = FindObjectOfType<MenuManager>();
                    if (menu != null)
                    {
                        menu.ActualizarNombreDesdePrefs();
                    }
                }
            }
            else
            {
                textoFeedback.text = "Error al editar: " + www.downloadHandler.text;
                Debug.Log("Respuesta completa: " + www.downloadHandler.text);
            }
        }
    }

    public void CerrarPanel()
    {
        gameObject.SetActive(false);
    }
}
