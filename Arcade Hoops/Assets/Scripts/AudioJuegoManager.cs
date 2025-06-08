using UnityEngine; // Importa el espacio de nombres necesario para trabajar con Unity

// Clase encargada de gestionar los sonidos del juego (ambiente y efectos)
public class AudioJuegoManager : MonoBehaviour
{
    // Instancia estática para implementar el patrón Singleton
    public static AudioJuegoManager Instance;

    // Fuente de audio para el sonido de fondo (ambiente)
    public AudioSource ambienteSource;

    // Fuente de audio para el sonido del enceste (cuando se anota)
    public AudioSource encesteSource;

    // Método que se ejecuta al iniciar el objeto (antes de Start)
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

    // Método que se llama automáticamente al comenzar la escena
    void Start()
    {
        // Reproduce el sonido de ambiente si está asignado (usando el operador null-condicional)
        ambienteSource?.Play();
    }

    // Método público para reproducir el sonido de enceste (llamado desde otros scripts)
    public void ReproducirEnceste()
    {
        // Si la fuente está asignada y no se está reproduciendo ya, la reproduce
        if (encesteSource != null && !encesteSource.isPlaying)
        {
            encesteSource.Play();
        }
    }
}
