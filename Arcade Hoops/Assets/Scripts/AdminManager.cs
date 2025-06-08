// Espacios de nombres necesarios para colecciones, tareas asincrónicas, red y UI
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.DTO; // Importa clases DTO como UsuarioDTO
using TMPro; // Para manejar TextMeshProUGUI
using UnityEngine.Networking; // Para hacer peticiones HTTP
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    // Clase que gestiona la interfaz y acciones del panel de administración
    public class AdminManager : MonoBehaviour
    {
        public GameObject itemPlantilla; // Elemento desactivado en el ScrollView que sirve como plantilla
        public Transform contenedorUsuarios; // Contenedor donde se instanciarán los elementos de usuario
        public TextMeshProUGUI feedbackTexto; // Texto que muestra mensajes de estado (éxito/error)

        private string apiUrl = "http://localhost:5195/api/auth/usuarios"; // URL de la API para obtener usuarios

        // Método que limpia el contenedor y comienza a cargar los usuarios
        public void CargarUsuarios()
        {
            // Elimina todos los elementos hijos del contenedor, excepto la plantilla
            foreach (Transform child in contenedorUsuarios)
            {
                if (child != itemPlantilla.transform)
                    Destroy(child.gameObject);
            }

            // Inicia la petición para obtener usuarios
            StartCoroutine(ObtenerUsuarios());
        }

        // Corrutina para realizar la petición GET y cargar los usuarios
        IEnumerator ObtenerUsuarios()
        {
            UnityWebRequest www = UnityWebRequest.Get(apiUrl);
            www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token")); // Añade token de autenticación
            www.SetRequestHeader("Accept", "application/json"); // Acepta JSON

            yield return www.SendWebRequest(); // Espera la respuesta

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Arregla el JSON para poder deserializarlo como array
                UsuarioDTO[] usuarios = JsonHelper.FromJson<UsuarioDTO>(FixJsonArray(www.downloadHandler.text));
                string emailActual = PlayerPrefs.GetString("email"); // Email del usuario actual (para no permitir autoeliminación)

                // Itera sobre los usuarios y crea un item para cada uno
                foreach (var usuario in usuarios)
                {
                    GameObject item = Instantiate(itemPlantilla, contenedorUsuarios);
                    item.SetActive(true); // Activa el item instanciado

                    // Asigna el texto con los datos del usuario
                    TextMeshProUGUI texto = item.transform.Find("NombreTexto").GetComponent<TextMeshProUGUI>();
                    texto.text = $"👤 {usuario.nombre} - {usuario.email} ({usuario.rol})";

                    // Referencia al botón de eliminar dentro del item
                    Button botonEliminarUsuarios = item.transform.Find("botonEliminarUsuarios").GetComponent<Button>();

                    // Evita que el usuario actual se elimine a sí mismo
                    if (usuario.email != emailActual)
                    {
                        // Asigna acción al botón
                        botonEliminarUsuarios.onClick.AddListener(() => EliminarUsuario(usuario.email));
                        botonEliminarUsuarios.gameObject.SetActive(true);
                    }
                    else
                    {
                        botonEliminarUsuarios.gameObject.SetActive(false);
                    }
                }

                feedbackTexto.text = "Usuarios cargados."; // Mensaje de éxito
            }
            else
            {
                feedbackTexto.text = "Error cargando usuarios."; // Mensaje de error
            }
        }

        // Método auxiliar para iniciar eliminación de usuario
        void EliminarUsuario(string email)
        {
            StartCoroutine(EliminarUsuarioCoroutine(email));
        }

        // Corrutina para eliminar el usuario mediante petición DELETE
        IEnumerator EliminarUsuarioCoroutine(string email)
        {
            // Construye la URL con el email codificado
            string url = $"http://localhost:5195/api/auth/eliminar?email={UnityWebRequest.EscapeURL(email)}";
            UnityWebRequest www = UnityWebRequest.Delete(url);
            www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token")); // Autenticación

            yield return www.SendWebRequest(); // Espera la respuesta

            if (www.result == UnityWebRequest.Result.Success)
            {
                feedbackTexto.text = $"✅ Usuario eliminado: {email}";
                CargarUsuarios(); // Recarga la lista para reflejar el cambio
            }
            else
            {
                feedbackTexto.text = $"❌ Error al eliminar: {www.downloadHandler.text}"; // Muestra mensaje de error
            }
        }

        // Método para adaptar el JSON de array plano a formato compatible con JsonHelper
        private string FixJsonArray(string json)
        {
            return "{\"usuarios\":" + json + "}"; // Envuélvelo con una propiedad de objeto para deserializar
        }
    }
}
