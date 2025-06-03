using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    private AudioSource sfxSource;
    public TMP_Text textoNombreJugador;

    public GameObject panelAbout;
    public GameObject panelGestionCuenta;
    public GameObject botonAdminPanel; 
    public GameObject panelAdminOpciones;
    public GameObject menuPrincipalUI;
    public GameObject panelOpciones;


    void Start()
    {
        sfxSource = GetComponent<AudioSource>();

        string nombre = PlayerPrefs.GetString("nombre", "Jugador");
        if (textoNombreJugador != null)
        {
            textoNombreJugador.text = "Bienvenido, " + nombre;
        }

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

    public void Logout()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("nombre");
        SceneManager.LoadScene("AuthScene");
    }

    public void ShowAbout()
    {
        panelAbout.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    public void CloseAbout()
    {
        panelAbout.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    public void URLLinkedin()
    {
        Application.OpenURL("https://www.linkedin.com/in/juan-carlos-d%C3%ADaz-moreno-6b4178151/");
    }

    public void ShowGestionCuenta()
    {
        panelGestionCuenta.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    public void CloseGestionCuenta()
    {
        panelGestionCuenta.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    public void AbrirPanelAdmin()
    {
        panelAdminOpciones.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    public void CerrarPanelAdmin()
    {
        panelAdminOpciones.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true);
        menuPrincipalUI.SetActive(false);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        menuPrincipalUI.SetActive(true);
    }

    public void Jugar()
    {
        Debug.Log("➡️ Llamando a IniciarNuevaPartida desde MenuManager");
        GameManager.Instance.IniciarNuevaPartida();
        SceneManager.LoadScene("GameScene");
    }

    public void ActualizarNombreDesdePrefs()
    {
        string nuevoNombre = PlayerPrefs.GetString("nombre", "Jugador");
        if (textoNombreJugador != null)
        {
            textoNombreJugador.text = "Bienvenido, " + nuevoNombre;
        }
    }

    public void OpenOptions()
    {
        sfxSource.PlayOneShot(clickSound);
        // Aquí puedes ocultar menuPrincipalUI si también tienes un panel de opciones
        // menuPrincipalUI.SetActive(false);
    }

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
