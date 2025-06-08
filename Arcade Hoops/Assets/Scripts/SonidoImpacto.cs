using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    using UnityEngine;

    // Esta clase reproduce un sonido cuando un objeto con el tag "Ball" colisiona con este objeto
    public class SonidoImpacto : MonoBehaviour
    {
        // Clip de audio que se reproducirá al impactar la pelota
        public AudioClip sonidoImpacto;

        // Fuente de audio asociada al objeto
        private AudioSource audioSource;

        // Se ejecuta al iniciar el objeto. Obtiene el componente AudioSource
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Se ejecuta automáticamente cuando ocurre una colisión con otro objeto
        private void OnCollisionEnter(Collision collision)
        {
            // Verifica si el objeto que colisiona tiene el tag "Ball"
            if (collision.gameObject.CompareTag("Ball"))
            {
                // Si hay un clip y un audio source, reproduce el sonido de impacto
                if (sonidoImpacto != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sonidoImpacto);
                }
            }
        }
    }
}
