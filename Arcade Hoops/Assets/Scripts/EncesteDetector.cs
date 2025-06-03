using UnityEngine;
using TMPro;
using System.Collections;

public class EncesteDetector : MonoBehaviour
{
    [Header("UI y Sonido")]
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public AudioClip scoreSound;
    public AudioClip voiceTwoPoints; // ← Voz "2 points"

    [Header("Feedback Visual")]
    public GameObject textoPuntos; // ← Texto "+2" que aparece sobre la canasta

    [Header("Animación Red")]
    public Animator animatorRed; // ← Animator de la red

    private int score = 0;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (textoPuntos != null)
            textoPuntos.SetActive(false); // Ocultar texto al inicio
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

                // Registrar tiro en API
                GameManager.Instance?.RegistrarTiro(true, posicionInicial, posicionFinal);

                // Sumar 2 puntos
                score += 2;
                if (scoreText != null)
                    scoreText.text = "Puntos: " + score;

                // Sonido de enceste
                if (audioSource != null && scoreSound != null)
                    audioSource.PlayOneShot(scoreSound);

                // Sonido de voz "2 points"
                if (audioSource != null && voiceTwoPoints != null)
                    audioSource.PlayOneShot(voiceTwoPoints);

                // Mostrar "+2"
                if (textoPuntos != null)
                    StartCoroutine(MostrarTextoPuntos());

                // Animación de la red
                if (animatorRed != null)
                    animatorRed.SetTrigger("Balancear");

                // Resetear balón
                lanzador.ResetBall();
            }
        }
    }

    private IEnumerator MostrarTextoPuntos()
    {
        textoPuntos.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        textoPuntos.SetActive(false);
    }
}
