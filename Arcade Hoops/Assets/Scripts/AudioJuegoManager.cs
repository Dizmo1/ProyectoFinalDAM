using UnityEngine; // Importa el espacio de nombres necesario para trabajar con Unity

// Clase encargada de gestionar los sonidos del juego (ambiente y efectos)
public class AudioJuegoManager : MonoBehaviour
{
    // Instancia est�tica para implementar el patr�n Singleton
    public static AudioJuegoManager Instance;

    // Fuente de audio para el sonido de fondo (ambiente)
    public AudioSource ambienteSource;

    // Fuente de audio para el sonido del enceste (cuando se anota)
    public AudioSource encesteSource;

    // M�todo que se ejecuta al iniciar el objeto (antes de Start)
    void Awake()
    {
        // Si no existe una instancia, se asigna esta y se marca para no destruir entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantiene este objeto al cambiar de escena
        }
        else
        {
            // Si ya existe otra instancia, destruye esta para evitar duplicados
            Destroy(gameObject);
        }
    }

    // M�todo que se llama autom�ticamente al comenzar la escena
    void Start()
    {
        // Reproduce el sonido de ambiente si est� asignado (usando el operador null-condicional)
        ambienteSource?.Play();
    }

    // M�todo p�blico para reproducir el sonido de enceste (llamado desde otros scripts)
    public void ReproducirEnceste()
    {
        // Si la fuente est� asignada y no se est� reproduciendo ya, la reproduce
        if (encesteSource != null && !encesteSource.isPlaying)
        {
            encesteSource.Play();
        }
    }
}
