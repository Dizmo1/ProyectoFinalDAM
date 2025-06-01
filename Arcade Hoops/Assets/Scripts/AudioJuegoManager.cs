using UnityEngine;

public class AudioJuegoManager : MonoBehaviour
{
    public static AudioJuegoManager Instance;

    public AudioSource ambienteSource;
    public AudioSource encesteSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional si quieres que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ambienteSource?.Play();
    }

    public void ReproducirEnceste()
    {
        if (encesteSource != null && !encesteSource.isPlaying)
        {
            encesteSource.Play();
        }
    }
}
