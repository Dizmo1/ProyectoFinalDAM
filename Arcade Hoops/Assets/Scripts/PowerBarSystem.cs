using UnityEngine;
using UnityEngine.UI;

public class PowerBarSystem : MonoBehaviour
{
    [Header("Configuraci�n de Barras")]
    public Slider powerSlider; // Barra de fuerza
    public Slider angleSlider; // Barra de direcci�n (�ngulo)
    public float maxForce = 30f;
    public float maxAngle = 45f; // �ngulo m�ximo en grados

    private Rigidbody rb;
    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        rb.isKinematic = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        rb.isKinematic = false;

        // Calcular direcci�n basada en el �ngulo
        float angle = angleSlider.value * maxAngle;
        Vector3 direction = Quaternion.Euler(angle, 0, 0) * Vector3.forward;

        // Aplicar fuerza basada en la barra
        float force = powerSlider.value * maxForce;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = initialPosition;
    }
}