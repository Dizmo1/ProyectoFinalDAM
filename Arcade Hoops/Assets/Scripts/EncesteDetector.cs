using UnityEngine;
using TMPro;
using System.Collections;

public class EncesteDetector : MonoBehaviour
{
    [Header("UI y Sonido")]
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public AudioClip scoreSound;

    [Header("Efectos visuales")]
    public GameObject fireworks;

    [Header("Animación Red")]
    public Animator animatorRed; // ← Asignar el Animator del objeto Net

    private int score = 0;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (fireworks != null)
            fireworks.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallController lanzador = other.GetComponent<BallController>();
            if (lanzador != null)
            {
                Vector3 posicionInicial = lanzador.GetLaunchPosition();
                Vector3 posicionFinal = other.transform.position;

                if (GameManager.Instance != null)
                {
                    Debug.Log("📤 Enviando tiro desde EncesteDetector...");
                    GameManager.Instance.RegistrarTiro(true, posicionInicial, posicionFinal);
                }
                else
                {
                    Debug.LogError("❌ GameManager.Instance es NULL en EncesteDetector.");
                }

                Debug.Log("✅ Enceste detectado y registrado");

                // 🎯 Sumar 2 puntos por canasta
                score += 2;
                if (scoreText != null)
                    scoreText.text = "Puntos: " + score;

                // 🔊 Sonido de enceste
                if (audioSource != null && scoreSound != null)
                    audioSource.PlayOneShot(scoreSound);

                // 🎆 Fuegos artificiales
                if (fireworks != null)
                {
                    fireworks.SetActive(true);
                    fireworks.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DesactivarFuegos());
                }

                // 🏀 Animación de la red
                if (animatorRed != null)
                {
                    animatorRed.SetTrigger("Balancear");
                }

                // 🔄 Resetear balón
                lanzador.ResetBall();
            }
        }
    }

    private IEnumerator DesactivarFuegos()
    {
        yield return new WaitForSeconds(2f);
        if (fireworks != null)
            fireworks.SetActive(false);
    }
}
