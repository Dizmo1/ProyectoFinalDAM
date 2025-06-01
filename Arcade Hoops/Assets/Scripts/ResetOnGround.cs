/*using UnityEngine;

public class ResetOnGround : MonoBehaviour
{
    [Header("Configuraci�n")]
    public Vector3 posicionInicial = new Vector3(0, 1.5f, 0); // Posici�n inicial del bal�n
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

    // Detectar colisi�n con el suelo
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        rb.isKinematic = true; // Desactiva la f�sica
        rb.velocity = Vector3.zero; // Resetea velocidad
        rb.angularVelocity = Vector3.zero; // Resetea rotaci�n
        transform.position = posicionInicial; // Vuelve a la posici�n inicial
    }
}*/