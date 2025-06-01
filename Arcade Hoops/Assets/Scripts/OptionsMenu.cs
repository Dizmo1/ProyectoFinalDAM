using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider musicSlider; // Asigna esto en el Inspector
    public Slider sfxSlider;   // Asigna esto en el Inspector
    public AudioSource musicSource; // Asigna esto en el Inspector

    void Start()
    {
        // Solo carga los valores si los sliders están asignados
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        }
    }

    public void OpenOptions()
    {
        Debug.Log("¡Abriendo el panel de opciones!");
        optionsPanel.SetActive(true);
        Debug.Log("Panel activo: " + optionsPanel.activeInHierarchy);
    }



    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}