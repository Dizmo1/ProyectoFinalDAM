using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro; // Para usar TextMeshProUGUI.
using UnityEngine; // Funcionalidades básicas de Unity.
using UnityEngine.SceneManagement; // Para cambiar de escena.

namespace Assets.Scripts
{
    // Clase que controla el temporizador de la partida
    public class GameTimer : MonoBehaviour
    {
        public float tiempoPartida = 60f; // Duración total de la partida en segundos (editable desde el inspector)
        public TextMeshProUGUI textoTemporizador; // Referencia al texto UI para mostrar el tiempo restante

        private bool tiempoActivo = true; // Controla si el temporizador está activo

        void Update()
        {
            // Solo cuenta atrás si el tiempo está activo
            if (tiempoActivo)
            {
                // Restar el tiempo que ha pasado desde el último frame
                tiempoPartida -= Time.deltaTime;

                // Convertir el tiempo restante a segundos enteros redondeando hacia arriba
                int segundosRestantes = Mathf.CeilToInt(tiempoPartida);

                // Mostrar el tiempo en el texto del UI
                textoTemporizador.text = segundosRestantes.ToString();

                // Si el tiempo se ha agotado
                if (tiempoPartida <= 0f)
                {
                    tiempoActivo = false; // Detener el temporizador
                    FinalizarPartida(); // Llamar al método que finaliza la partida
                }
            }
        }

        // Método llamado cuando se agota el tiempo
        private void FinalizarPartida()
        {
            Debug.Log("⏰ Tiempo terminado. Finalizando partida...");

            // Notificar al GameManager que finalice la partida
            GameManager.Instance.FinalizarPartida();
        }
    }
}
