using UnityEngine;
using UnityEngine.UI;

public class OpcionesManager : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderEfectos;
    public Toggle toggleMusica;
    public Toggle toggleEfectos;

    private AudioSource musica;
    private AudioSource efectos;

    void Start()
    {
        musica = GameObject.Find("music").GetComponent<AudioSource>();
        efectos = GameObject.Find("musicClick").GetComponent<AudioSource>();

        // Inicializar valores
        sliderMusica.value = musica.volume;
        sliderEfectos.value = efectos.volume;

        toggleMusica.isOn = musica.volume > 0;
        toggleEfectos.isOn = efectos.volume > 0;

        // Listeners
        sliderMusica.onValueChanged.AddListener((v) => musica.volume = v);
        sliderEfectos.onValueChanged.AddListener((v) => efectos.volume = v);

        toggleMusica.onValueChanged.AddListener((activo) => musica.volume = activo ? sliderMusica.value : 0);
        toggleEfectos.onValueChanged.AddListener((activo) => efectos.volume = activo ? sliderEfectos.value : 0);
    }
}
