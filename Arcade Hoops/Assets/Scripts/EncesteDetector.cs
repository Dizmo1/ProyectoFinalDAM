using UnityEngine;
using TMPro;
using System.Collections;

public class EncesteDetector : MonoBehaviour
{
    [Header("UI y Sonido")]
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public AudioClip scoreSound;
    public AudioClip voiceTwoPoints; 

    [Header("Feedback Visual")]
    public GameObject textoPuntos; 

    [Header("Animación Red")]
    public Animator animatorRed; 

    private int score = 0;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (textoPuntos != null)
            textoPuntos.SetActive(false);
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

                
                score += 2;
                if (scoreText != null)
                    scoreText.text = "Puntuación: " + score;

                GameManager.Instance?.SumarPuntos(2);



                if (audioSource != null && scoreSound != null)
                    audioSource.PlayOneShot(scoreSound);

                
                if (audioSource != null && voiceTwoPoints != null)
                    audioSource.PlayOneShot(voiceTwoPoints);

                
                if (textoPuntos != null)
                    StartCoroutine(MostrarTextoPuntos());

                
                if (animatorRed != null)
                    animatorRed.SetTrigger("Balancear");

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
