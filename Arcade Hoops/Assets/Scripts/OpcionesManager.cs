// Necesario para acceder a componentes de Unity
using UnityEngine;
// Necesario para utilizar Sliders y Toggles de la UI
using UnityEngine.UI;

// Clase que gestiona los ajustes de audio en el menú de opciones
public class OpcionesManager : MonoBehaviour
{
    // Referencias a los sliders para controlar el volumen
    public Slider sliderMusica;
    public Slider sliderEfectos;

    // Referencias a los toggles para activar/desactivar el sonido
    public Toggle toggleMusica;
    public Toggle toggleEfectos;

    // AudioSources para la música de fondo y los efectos
    private AudioSource musica;
    private AudioSource efectos;

    // Método que se llama al inicio de la escena
    void Start()
    {
        // Busca los objetos "music" y "musicClick" en la escena y obtiene sus componentes de AudioSource
        musica = GameObject.Find("music").GetComponent<AudioSource>();
        efectos = GameObject.Find("musicClick").GetComponent<AudioSource>();

        // Inicializa los valores de los sliders con el volumen actual de cada audio
        sliderMusica.value = musica.volume;
        sliderEfectos.value = efectos.volume;

        // Inicializa los toggles en función de si el volumen es mayor que 0
        toggleMusica.isOn = musica.volume > 0;
        toggleEfectos.isOn = efectos.volume > 0;

        // Asigna listeners para que al mover los sliders se actualice el volumen correspondiente
        sliderMusica.onValueChanged.AddListener((v) => musica.volume = v);
        sliderEfectos.onValueChanged.AddListener((v) => efectos.volume = v);

        // Asigna listeners para que al activar/desactivar los toggles se silencie o se reactive el audio
        toggleMusica.onValueChanged.AddListener((activo) => musica.volume = activo ? sliderMusica.value : 0);
        toggleEfectos.onValueChanged.AddListener((activo) => efectos.volume = activo ? sliderEfectos.value : 0);
    }
}
