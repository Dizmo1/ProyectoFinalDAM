using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class ResumenPartidaManager : MonoBehaviour
    {
        public GameObject panelResumen;
        public GameObject uiJuegoBase;  // ← Asigna esto desde el Inspector

        public TextMeshProUGUI textoNombreJugador;
        public TextMeshProUGUI textoPuntos;
        public TextMeshProUGUI textoTiempo;

        public void MostrarResumen(string nombre, int puntos, float tiempo)
        {
            if (uiJuegoBase != null)
                uiJuegoBase.SetActive(false); // Oculta la UI de juego

            panelResumen.SetActive(true); // Muestra el resumen

            textoNombreJugador.text = "👤 Jugador: " + nombre;
            textoPuntos.text = "🏀 Puntos: " + puntos;
            textoTiempo.text = $"⏱️ Tiempo: {tiempo:F1} segundos";
        }

        public void VolverAlMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
