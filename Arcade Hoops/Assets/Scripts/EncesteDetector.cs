using UnityEngine;
using TMPro;

public class EncesteDetector : MonoBehaviour
{
    [Header("UI y Sonido")]
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public AudioClip scoreSound;

    private int score = 0;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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

                // Registrar tiro en la API
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

                // Sumar puntuación
                score++;
                if (scoreText != null)
                    scoreText.text = "Puntos: " + score;

                // Sonido
                if (audioSource != null && scoreSound != null)
                    audioSource.PlayOneShot(scoreSound);

                // Resetear balón
                lanzador.ResetBall();
            }
        }
    }
}
