using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    using TMPro;
    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI textoPuntuacion;

        private int puntos = 0;

        private void OnEnable()
        {
            GameManager.Instance.OnTiroRegistrado.AddListener(ActualizarPuntuacion);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnTiroRegistrado.RemoveListener(ActualizarPuntuacion);
        }

        void ActualizarPuntuacion(bool acierto, float distancia)
        {
            if (acierto)
            {
                puntos++;
                textoPuntuacion.text = "Puntuación: " + puntos;
            }
        }
    }


}
