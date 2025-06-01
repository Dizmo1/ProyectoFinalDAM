using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{

    public class JuegoInitializer : MonoBehaviour
    {
        void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IniciarNuevaPartida();
                Debug.Log("✅ Partida iniciada automáticamente al entrar en la escena.");
            }
        }
    }

}
