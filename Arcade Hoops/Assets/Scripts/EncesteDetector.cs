using UnityEngine; // Importa funciones del motor Unity.
using TMPro; // Permite trabajar con componentes de texto de TextMeshPro.
using System.Collections; // Necesario para usar corrutinas (IEnumerator).

public class EncesteDetector : MonoBehaviour // Clase que detecta cuándo se encesta el balón.
{
    [Header("UI y Sonido")] // Agrupa estas variables en el inspector bajo el encabezado "UI y Sonido"
    public TextMeshProUGUI scoreText; // Referencia al texto que muestra la puntuación.
    public AudioSource audioSource; // Fuente de audio para reproducir sonidos.
    public AudioClip scoreSound; // Clip de sonido que se reproduce al encestar.
    public AudioClip voiceTwoPoints; // Clip de voz que dice "2 puntos".

    [Header("Feedback Visual")] // Encabezado visual en el inspector.
    public GameObject textoPuntos; // GameObject de texto que muestra "+2" temporalmente.

    [Header("Animación Red")] // Otro encabezado para la parte de animación.
    public Animator animatorRed; // Referencia al Animator de la red para animarla cuando encesta.

    private int score = 0; // Puntuación actual del jugador en esta sesión visual (no necesariamente sincronizada con GameManager).

    private void Start()
    {
        // Si no se ha asignado audioSource manualmente, se intenta obtener el componente en el mismo GameObject.
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Si se ha asignado un texto de puntos, se oculta al iniciar.
        if (textoPuntos != null)
            textoPuntos.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entró en el trigger tiene la etiqueta "Ball"
        if (other.CompareTag("Ball"))
        {
            // Obtiene el script BallController del objeto
            BallController lanzador = other.GetComponent<BallController>();
            if (lanzador != null)
            {
                // Obtiene la posición de lanzamiento y la posición actual del balón
                Vector3 posicionInicial = lanzador.GetLaunchPosition();
                Vector3 posicionFinal = other.transform.position;

                // Llama a GameManager para registrar el tiro como acierto
                GameManager.Instance?.RegistrarTiro(true, posicionInicial, posicionFinal);

                // Suma 2 puntos a la puntuación local
                score += 2;
                if (scoreText != null)
                    scoreText.text = "Puntuación: " + score;

                // Suma 2 puntos al GameManager (sistema global)
                GameManager.Instance?.SumarPuntos(2);

                // Reproduce el sonido de enceste si está disponible
                if (audioSource != null && scoreSound != null)
                    audioSource.PlayOneShot(scoreSound);

                // Reproduce el clip de voz de "dos puntos" si está disponible
                if (audioSource != null && voiceTwoPoints != null)
                    audioSource.PlayOneShot(voiceTwoPoints);

                // Muestra el texto "+2" durante 1.2 segundos
                if (textoPuntos != null)
                    StartCoroutine(MostrarTextoPuntos());

                // Activa la animación de la red si se ha asignado el Animator
                if (animatorRed != null)
                    animatorRed.SetTrigger("Balancear");

                // Resetea la pelota para volver a lanzar
                lanzador.ResetBall();
            }
        }
    }

    private IEnumerator MostrarTextoPuntos()
    {
        // Activa el texto de puntos
        textoPuntos.SetActive(true);
        // Espera 1.2 segundos
        yield return new WaitForSeconds(1.2f);
        // Oculta el texto de puntos
        textoPuntos.SetActive(false);
    }
}
