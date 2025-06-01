using UnityEngine;
using UnityEngine.UI;

public class Puntuacion : MonoBehaviour {
    public Text textoPuntos;
    private int puntos = 0;

    public void SumarPuntos(int cantidad) {
        puntos += cantidad;
        textoPuntos.text = "Puntos: " + puntos;
    }
}