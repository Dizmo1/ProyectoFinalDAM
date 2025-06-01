/*using UnityEngine;

public class ResetOnGround : MonoBehaviour
{
    [Header("Configuración")]
    public Vector3 posicionInicial = new Vector3(0, 1.5f, 0); // Posición inicial del balón
    public float alturaMinima = -10f; // Altura para resetear si cae fuera del mapa

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Resetear si cae fuera del mapa
        if (transform.position.y < alturaMinima)
        {
            ResetBall();
        }
    }

    // Detectar colisión con el suelo
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        rb.isKinematic = true; // Desactiva la física
        rb.velocity = Vector3.zero; // Resetea velocidad
        rb.angularVelocity = Vector3.zero; // Resetea rotación
        transform.position = posicionInicial; // Vuelve a la posición inicial
    }
}*/