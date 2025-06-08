using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    using TMPro;
    using UnityEngine;

    // Clase que gestiona la actualización de la UI de puntuación durante el juego
    public class UIManager : MonoBehaviour
    {
        // Referencia al texto donde se mostrará la puntuación
        public TextMeshProUGUI textoPuntuacion;

        // Variable para llevar el conteo de puntos
        private int puntos = 0;

        // Se llama cuando el objeto se activa. Se suscribe al evento OnTiroRegistrado del GameManager
        private void OnEnable()
        {
            GameManager.Instance.OnTiroRegistrado.AddListener(ActualizarPuntuacion);
        }

        // Se llama cuando el objeto se desactiva. Se desuscribe del evento para evitar referencias inválidas
        private void OnDisable()
        {
            GameManager.Instance.OnTiroRegistrado.RemoveListener(ActualizarPuntuacion);
        }

        // Método llamado cuando se registra un tiro. Solo suma puntos si fue un acierto
        void ActualizarPuntuacion(bool acierto, float distancia)
        {
            if (acierto)
            {
                puntos++; // Suma 1 punto por acierto (visual, no real)
                textoPuntuacion.text = "Puntuación: " + puntos;
            }
        }
    }
}
