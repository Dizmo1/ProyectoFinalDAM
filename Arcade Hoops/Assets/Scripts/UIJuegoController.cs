using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    

    public class UIJuegoController : MonoBehaviour
    {
        public void SalirAlMenu()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.FinalizarPartida(); 
            }
            else
            {
                Debug.LogWarning("⚠️ GameManager no encontrado al salir.");
                SceneManager.LoadScene("MenuScene"); 
            }
        }



    }

}
