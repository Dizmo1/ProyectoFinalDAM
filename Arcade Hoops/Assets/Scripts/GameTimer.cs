using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
   
    public class GameTimer : MonoBehaviour
    {
        public float tiempoPartida = 60f; // Duración en segundos
        public TextMeshProUGUI textoTemporizador;

        private bool tiempoActivo = true;

        void Update()
        {
            if (tiempoActivo)
            {
                tiempoPartida -= Time.deltaTime;

                int segundosRestantes = Mathf.CeilToInt(tiempoPartida);
                textoTemporizador.text = segundosRestantes.ToString();

                if (tiempoPartida <= 0f)
                {
                    tiempoActivo = false;
                    FinalizarPartida();
                }
            }
        }

        private void FinalizarPartida()
        {
            Debug.Log("⏰ Tiempo terminado. Finalizando partida...");
            GameManager.Instance.FinalizarPartida();
            SceneManager.LoadScene("MenuScene");
        }
    }

}
