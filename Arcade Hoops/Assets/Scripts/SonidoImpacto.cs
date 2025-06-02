using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    using UnityEngine;

    public class SonidoImpacto : MonoBehaviour
    {
        public AudioClip sonidoImpacto;

        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                if (sonidoImpacto != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sonidoImpacto);
                }
            }
        }
    }

}
