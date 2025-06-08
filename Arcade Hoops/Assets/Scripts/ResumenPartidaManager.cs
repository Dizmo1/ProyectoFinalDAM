// Librerías necesarias para UI y manejo de escenas
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    // Clase que gestiona la pantalla de resumen tras una partida
    public class ResumenPartidaManager : MonoBehaviour
    {
        // Referencia al panel de resumen que se activa tras la partida
        public GameObject panelResumen;

        // UI principal del juego que debe ocultarse cuando aparece el resumen
        public GameObject uiJuegoBase;  // ← Asignar en el Inspector de Unity

        // Textos del resumen para mostrar nombre, puntos y tiempo
        public TextMeshProUGUI textoNombreJugador;
        public TextMeshProUGUI textoPuntos;
        public TextMeshProUGUI textoTiempo;

        // Método para mostrar el resumen tras finalizar la partida
        public void MostrarResumen(string nombre, int puntos, float tiempo)
        {
            // Oculta la UI del juego para que no se vea detrás del resumen
            if (uiJuegoBase != null)
                uiJuegoBase.SetActive(false);

            // Activa el panel con los datos del resumen
            panelResumen.SetActive(true);

            // Rellena los campos de texto con la información del jugador
            textoNombreJugador.text = "Jugador: " + nombre;
            textoPuntos.text = "Puntos: " + puntos;
            textoTiempo.text = $"Tiempo: {tiempo:F1} segundos"; // Tiempo formateado con 1 decimal
        }

        // Método llamado desde un botón para volver al menú principal
        public void VolverAlMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
