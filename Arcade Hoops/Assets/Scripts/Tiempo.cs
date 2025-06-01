using UnityEngine;
using UnityEngine.UI;

public class Tiempo : MonoBehaviour
{
    private Text textoTiempo;
    private float tiempoRestante = 60f;

    void Start()
    {
        textoTiempo = GetComponent<Text>();
    }

    void Update()
    {
        if (textoTiempo != null)
        {
            tiempoRestante -= Time.deltaTime;
            textoTiempo.text = "Tiempo: " + Mathf.Round(tiempoRestante);
        }
    }
}
