using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.DTO;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AdminManager : MonoBehaviour
    {
        public GameObject itemPlantilla; // El Text desactivado como plantilla
        public Transform contenedorUsuarios; // El Content del Scroll View
        public TextMeshProUGUI feedbackTexto;

        private string apiUrl = "http://localhost:5195/api/auth/usuarios";

        public void CargarUsuarios()
        {
            foreach (Transform child in contenedorUsuarios)
            {
                if (child != itemPlantilla.transform)
                    Destroy(child.gameObject);
            }

            StartCoroutine(ObtenerUsuarios());
        }

        IEnumerator ObtenerUsuarios()
        {
            UnityWebRequest www = UnityWebRequest.Get(apiUrl);
            www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                UsuarioDTO[] usuarios = JsonHelper.FromJson<UsuarioDTO>(FixJsonArray(www.downloadHandler.text));
                string emailActual = PlayerPrefs.GetString("email");

                foreach (var usuario in usuarios)
                {
                    GameObject item = Instantiate(itemPlantilla, contenedorUsuarios);
                    item.SetActive(true);

                    // Texto del usuario
                    TextMeshProUGUI texto = item.transform.Find("NombreTexto").GetComponent<TextMeshProUGUI>();
                    texto.text = $"👤 {usuario.nombre} - {usuario.email} ({usuario.rol})";

                    // Botón eliminar
                    Button botonEliminarUsuarios = item.transform.Find("botonEliminarUsuarios").GetComponent<Button>();

                    if (usuario.email != emailActual)
                    {
                        botonEliminarUsuarios.onClick.AddListener(() => EliminarUsuario(usuario.email));
                        botonEliminarUsuarios.gameObject.SetActive(true);
                    }
                    else
                    {
                        botonEliminarUsuarios.gameObject.SetActive(false);
                    }
                }

                feedbackTexto.text = "Usuarios cargados.";
            }
            else
            {
                feedbackTexto.text = "Error cargando usuarios.";
            }
        }

        void EliminarUsuario(string email)
        {
            StartCoroutine(EliminarUsuarioCoroutine(email));
        }

        IEnumerator EliminarUsuarioCoroutine(string email)
        {
            string url = $"http://localhost:5195/api/auth/eliminar?email={UnityWebRequest.EscapeURL(email)}";
            UnityWebRequest www = UnityWebRequest.Delete(url);
            www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                feedbackTexto.text = $"✅ Usuario eliminado: {email}";
                CargarUsuarios(); // Recargar lista tras eliminar
            }
            else
            {
                feedbackTexto.text = $"❌ Error al eliminar: {www.downloadHandler.text}";
            }
        }

        private string FixJsonArray(string json)
        {
            return "{\"usuarios\":" + json + "}";
        }
    }
}
