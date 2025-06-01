using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Configuración de Lanzamiento")]
    [SerializeField] private float maxForce = 20f;
    [SerializeField] private float minDragDistance = 50f;
    [SerializeField] private float baseArcHeight = 1.8f;
    [SerializeField] private float horizontalSensitivity = 0.03f;
    [SerializeField] private float verticalSensitivity = 0.015f;

    [Header("Feedback Visual")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 25;
    [SerializeField] private float simulationStep = 0.1f;
    [SerializeField] private ParticleSystem launchEffect;
    [SerializeField] private Transform targetPoint;


    private Rigidbody _rb;
    private Vector3 _initialPosition;
    private bool _isDragging = false;
    private Vector3 _dragStartPos;
    private Vector3 _launchPosition;
    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        InitializeBall();
    }

    private void InitializeBall()
    {
        _initialPosition = transform.position;
        _rb.isKinematic = true;
        trajectoryLine.enabled = false;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) StartDrag();
        if (Input.GetMouseButtonUp(0)) EndDrag();
        if (_isDragging) UpdateTrajectory();
    }

    private void StartDrag()
    {
        _isDragging = true;
        _dragStartPos = Input.mousePosition;
        trajectoryLine.enabled = true;
    }

    private void UpdateTrajectory()
    {
        Vector3 dragVector = GetDragVector();
        Vector3 launchDirection = CalculateLaunchDirection(dragVector);
        float launchForce = CalculateLaunchForce(dragVector.magnitude);

        DrawTrajectory(launchDirection * launchForce);
    }

    private Vector3 GetDragVector()
    {
        return Input.mousePosition - _dragStartPos;
    }

    private Vector3 CalculateLaunchDirection(Vector3 dragVector)
    {
        Vector3 screenToWorldDirection = new Vector3(
            dragVector.x * horizontalSensitivity,
            (dragVector.y * verticalSensitivity) + baseArcHeight,
            1f // fuerza fija hacia adelante, más fiable que usar camera.forward.y
        );

        return _mainCamera.transform.TransformDirection(screenToWorldDirection).normalized;
    }

    private float CalculateLaunchForce(float dragMagnitude)
    {
        return Mathf.Clamp(dragMagnitude * 0.1f, 0, maxForce);
    }

    private void DrawTrajectory(Vector3 force)
    {
        trajectoryLine.positionCount = trajectoryPoints;
        Vector3 velocity = force / _rb.mass;
        Vector3 position = transform.position;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            trajectoryLine.SetPosition(i, position);
            velocity += Physics.gravity * simulationStep;
            position += velocity * simulationStep;
        }

        // 🟢 Posiciona el TargetPoint al final de la trayectoria
        if (targetPoint != null)
        {
            targetPoint.position = position;
        }

    }

    private void EndDrag()
    {
        _isDragging = false;
        trajectoryLine.enabled = false;

        if (targetPoint != null)
        {
            LaunchBallToTarget(targetPoint.position);
            PlayLaunchEffects();
        }
    }

    private void LaunchBallToTarget(Vector3 target)
    {
        _rb.isKinematic = false;
        _launchPosition = transform.position;

        Vector3 startPos = transform.position;
        Vector3 targetPos = target;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float height = Mathf.Max(targetPos.y - startPos.y, 1f); // altura relativa
        float arcHeight = height + 2.5f; // puedes ajustar esta altura extra

        // Calcular tiempo de subida
        float timeToApex = Mathf.Sqrt(2 * arcHeight / gravity);
        float totalTime = timeToApex + Mathf.Sqrt(2 * (arcHeight - height) / gravity);

        // Calcular velocidad inicial
        Vector3 horizontal = new Vector3(targetPos.x - startPos.x, 0f, targetPos.z - startPos.z);
        Vector3 horizontalVelocity = horizontal / totalTime;
        float verticalVelocity = Mathf.Sqrt(2 * gravity * arcHeight);

        Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        _rb.velocity = launchVelocity;
    }

    private void PlayLaunchEffects()
    {
        if (launchEffect != null)
        {
            launchEffect.Play();
        }
    }

    public void ResetBall()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = _initialPosition;
        _rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            ResetBall();
        }
    }

    public Vector3 GetLaunchPosition()
    {
        return _launchPosition;
    }
}
