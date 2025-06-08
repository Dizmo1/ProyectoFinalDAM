using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    // Controlador de la UI del juego, permite salir al menú principal
    public class UIJuegoController : MonoBehaviour
    {
        // Método público que se puede enlazar desde un botón para salir al menú
        public void SalirAlMenu()
        {
            // Si el GameManager existe, finaliza la partida actual de forma correcta
            if (GameManager.Instance != null)
            {
                GameManager.Instance.FinalizarPartida();
            }
            else
            {
                // Si no existe, lanza advertencia y fuerza carga del menú
                Debug.LogWarning("⚠️ GameManager no encontrado al salir.");
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}
