// Importa el espacio de nombres para trabajar con texto de TextMeshPro
using TMPro;
// Importa funcionalidades básicas de Unity
using UnityEngine;
// Permite cambiar de escena
using UnityEngine.SceneManagement;

// Clase que gestiona el menú principal del juego
public class MenuManager : MonoBehaviour
{
    // Clip de sonido para clics de botón
    [SerializeField] private AudioClip clickSound;

    // Fuente de audio para reproducir efectos de sonido
    private AudioSource sfxSource;

    // Texto que muestra el nombre del jugador
    public TMP_Text textoNombreJugador;

    // Panel "Sobre mí"
    public GameObject panelAbout;
    // Panel de gestión de cuenta del usuario
    public GameObject panelGestionCuenta;
    // Botón que lleva al panel de administración (visible solo para admins)
    public GameObject botonAdminPanel;
    // Panel con opciones de administración
    public GameObject panelAdminOpciones;
    // Contenedor principal de botones del menú
    public GameObject menuPrincipalUI;
    // Panel de opciones del juego (volumen, etc.)
    public GameObject panelOpciones;

    // Método llamado al iniciar la escena
    void Start()
    {
        // Asigna el componente AudioSource
        sfxSource = GetComponent<AudioSource>();

        // Recupera el nombre del jugador desde PlayerPrefs
        string nombre = PlayerPrefs.GetString("nombre", "Jugador");
        if (textoNombreJugador != null)
        {
            textoNombreJugador.text = "Bienvenido, " + nombre;
        }

        // Recupera el rol del jugador y muestra/oculta el botón admin
        string rol = PlayerPrefs.GetString("rol", "jugador");
        if (rol == "admin" && botonAdminPanel != null)
        {
            botonAdminPanel.SetActive(true);
        }
        else if (botonAdminPanel != null)
        {
            botonAdminPanel.SetActive(false);
        }
    }

    // Cierra la sesión y vuelve a la pantalla de autenticación
    public void Logout()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("nombre");
        SceneManager.LoadScene("AuthScene");
    }

    // Muestra el panel "Sobre mí"
    public void ShowAbout()
    {
        panelAbout.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    // Cierra el panel "Sobre mí" y vuelve al menú principal
    public void CloseAbout()
    {
        panelAbout.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    // Abre el perfil de LinkedIn en el navegador
    public void URLLinkedin()
    {
        Application.OpenURL("https://www.linkedin.com/in/juan-carlos-d%C3%ADaz-moreno-6b4178151/");
    }

    // Muestra el panel para gestionar cuenta
    public void ShowGestionCuenta()
    {
        panelGestionCuenta.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    // Cierra el panel de gestión de cuenta
    public void CloseGestionCuenta()
    {
        panelGestionCuenta.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    // Muestra el panel con opciones de administración
    public void AbrirPanelAdmin()
    {
        panelAdminOpciones.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    // Cierra el panel de administración
    public void CerrarPanelAdmin()
    {
        panelAdminOpciones.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    // Muestra el panel de opciones del juego
    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    // Cierra el panel de opciones
    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    // Inicia una nueva partida desde el GameManager y cambia a la escena de juego
    public void Jugar()
    {
        Debug.Log("➡️ Llamando a IniciarNuevaPartida desde MenuManager");
        GameManager.Instance.IniciarNuevaPartida();
        SceneManager.LoadScene("GameScene");
    }

    // Actualiza el texto del nombre del jugador desde PlayerPrefs
    public void ActualizarNombreDesdePrefs()
    {
        string nuevoNombre = PlayerPrefs.GetString("nombre", "Jugador");
        if (textoNombreJugador != null)
        {
            textoNombreJugador.text = "Bienvenido, " + nuevoNombre;
        }
    }

    // Reproduce sonido de clic y (opcionalmente) abre un panel de opciones
    public void OpenOptions()
    {
        sfxSource.PlayOneShot(clickSound);
        // Puedes ocultar el menú principal si lo necesitas
        // menuPrincipalUI.SetActive(false);
    }

    // Cierra la aplicación (también detiene la ejecución si estás en el editor)
    public void QuitGame()
    {
        if (clickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
