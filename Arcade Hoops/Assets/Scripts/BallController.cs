using UnityEngine; // Importa la API de Unity para manejar objetos del juego, físicas, entradas, etc.

[RequireComponent(typeof(Rigidbody))] // Obliga a que este componente tenga un Rigidbody en el GameObject
public class BallController : MonoBehaviour
{
    [Header("Configuración de Lanzamiento")] // Agrupa variables en el Inspector de Unity
    [SerializeField] private float maxForce = 20f; // Fuerza máxima que se puede aplicar al lanzar
    [SerializeField] private float minDragDistance = 50f; // Distancia mínima de arrastre para considerar el lanzamiento
    [SerializeField] private float baseArcHeight = 1.8f; // Altura base del arco del disparo
    [SerializeField] private float horizontalSensitivity = 0.03f; // Sensibilidad horizontal del arrastre
    [SerializeField] private float verticalSensitivity = 0.015f; // Sensibilidad vertical del arrastre

    [Header("Feedback Visual")]
    [SerializeField] private LineRenderer trajectoryLine; // Línea para mostrar la trayectoria prevista del tiro
    [SerializeField] private int trajectoryPoints = 25; // Número de puntos en la trayectoria
    [SerializeField] private float simulationStep = 0.1f; // Paso de simulación para calcular la trayectoria
    [SerializeField] private Transform targetPoint; // Punto objetivo al final de la trayectoria

    private Rigidbody _rb; // Referencia al Rigidbody del balón
    private Vector3 _initialPosition; // Posición inicial del balón
    private bool _isDragging = false; // Si el jugador está arrastrando para lanzar
    private Vector3 _dragStartPos; // Posición inicial del mouse al arrastrar
    private Vector3 _launchPosition; // Posición desde donde se lanza el balón
    private Camera _mainCamera; // Referencia a la cámara principal

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // Obtiene el Rigidbody del objeto
        _mainCamera = Camera.main; // Obtiene la cámara principal
        InitializeBall(); // Inicializa el estado del balón
    }

    private void InitializeBall()
    {
        _initialPosition = transform.position; // Guarda la posición inicial
        _rb.isKinematic = true; // Desactiva la física hasta que se lance
        trajectoryLine.enabled = false; // Oculta la línea de trayectoria al principio
    }

    private void Update()
    {
        HandleInput(); // Detecta entradas del usuario en cada frame
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) StartDrag(); // Si se presiona el botón izquierdo del mouse, empieza el arrastre
        if (Input.GetMouseButtonUp(0)) EndDrag(); // Si se suelta el botón, termina el arrastre y lanza
        if (_isDragging) UpdateTrajectory(); // Si se está arrastrando, actualiza la trayectoria
    }

    private void StartDrag()
    {
        _isDragging = true; // Activa el modo de arrastre
        _dragStartPos = Input.mousePosition; // Guarda la posición inicial del mouse
        trajectoryLine.enabled = true; // Muestra la trayectoria mientras se arrastra
    }

    private void UpdateTrajectory()
    {
        Vector3 dragVector = GetDragVector(); // Calcula el vector de arrastre
        Vector3 launchDirection = CalculateLaunchDirection(dragVector); // Dirección del lanzamiento
        float launchForce = CalculateLaunchForce(dragVector.magnitude); // Fuerza del lanzamiento

        DrawTrajectory(launchDirection * launchForce); // Dibuja la trayectoria
    }

    private Vector3 GetDragVector()
    {
        return Input.mousePosition - _dragStartPos; // Calcula el vector desde donde empezó el arrastre hasta donde está el mouse
    }

    private Vector3 CalculateLaunchDirection(Vector3 dragVector)
    {
        Vector3 screenToWorldDirection = new Vector3(
            dragVector.x * horizontalSensitivity, // Dirección horizontal
            (dragVector.y * verticalSensitivity) + baseArcHeight, // Dirección vertical + arco base
            1f // Siempre hacia adelante
        );

        return _mainCamera.transform.TransformDirection(screenToWorldDirection).normalized; // Convierte a dirección en el mundo
    }

    private float CalculateLaunchForce(float dragMagnitude)
    {
        return Mathf.Clamp(dragMagnitude * 0.1f, 0, maxForce); // Limita la fuerza de lanzamiento entre 0 y maxForce
    }

    private void DrawTrajectory(Vector3 force)
    {
        trajectoryLine.positionCount = trajectoryPoints; // Número de puntos a mostrar
        Vector3 velocity = force / _rb.mass; // Velocidad inicial
        Vector3 position = transform.position; // Posición inicial

        for (int i = 0; i < trajectoryPoints; i++)
        {
            trajectoryLine.SetPosition(i, position); // Establece cada punto de la línea
            velocity += Physics.gravity * simulationStep; // Aplica gravedad al siguiente paso
            position += velocity * simulationStep; // Calcula la siguiente posición
        }

        // 🟢 Mueve el targetPoint al final de la trayectoria
        if (targetPoint != null)
        {
            targetPoint.position = position;
        }
    }

    private void EndDrag()
    {
        _isDragging = false; // Ya no se está arrastrando
        trajectoryLine.enabled = false; // Oculta la línea de trayectoria

        if (targetPoint != null)
        {
            LaunchBallToTarget(targetPoint.position); // Lanza el balón al punto objetivo
        }
    }

    private void LaunchBallToTarget(Vector3 target)
    {
        _rb.isKinematic = false; // Activa la física
        _launchPosition = transform.position; // Guarda desde dónde se lanzó

        Vector3 startPos = transform.position;
        Vector3 targetPos = target;

        float gravity = Mathf.Abs(Physics.gravity.y); // Gravedad positiva
        float height = Mathf.Max(targetPos.y - startPos.y, 1f); // Diferencia de altura
        float arcHeight = height + 2.5f; // Altura adicional para el arco del tiro

        float timeToApex = Mathf.Sqrt(2 * arcHeight / gravity); // Tiempo hasta el punto más alto
        float totalTime = timeToApex + Mathf.Sqrt(2 * (arcHeight - height) / gravity); // Tiempo total

        Vector3 horizontal = new Vector3(targetPos.x - startPos.x, 0f, targetPos.z - startPos.z); // Vector horizontal
        Vector3 horizontalVelocity = horizontal / totalTime; // Velocidad horizontal
        float verticalVelocity = Mathf.Sqrt(2 * gravity * arcHeight); // Velocidad vertical

        Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity; // Velocidad total

        _rb.velocity = launchVelocity; // Aplica la velocidad al Rigidbody
    }

    public void ResetBall()
    {
        _rb.velocity = Vector3.zero; // Detiene el movimiento
        _rb.angularVelocity = Vector3.zero; // Detiene la rotación
        transform.position = _initialPosition; // Vuelve a la posición inicial
        _rb.isKinematic = true; // Desactiva la física
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) // Si toca el suelo
        {
            ResetBall(); // Reinicia la posición del balón
        }
    }

    public Vector3 GetLaunchPosition()
    {
        return _launchPosition; // Devuelve la posición desde la que se lanzó
    }
}
