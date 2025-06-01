using UnityEngine;

public class FreeDragThrow : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Multiplicador general de la fuerza del lanzamiento")]
    public float fuerzaMultiplicador = 0.5f;

    [Tooltip("Distancia máxima de arrastre en píxeles para fuerza máxima")]
    public float maxDragDistance = 300f;

    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 dragStartPosition;
    private bool isDragging = false;
    private float cooldown = 0.5f; // Tiempo de espera entre lanzamientos
    private float lastThrowTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        rb.isKinematic = true;
    }

    void Update()
    {
        if (Time.time < lastThrowTime + cooldown) return;

        // Iniciar arrastre
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                dragStartPosition = Input.mousePosition;
            }
        }

        // Dibujar línea de dirección mientras se arrastra
        if (isDragging)
        {
            DrawDragLine();
        }

        // Lanzar al soltar
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector3 dragEndPosition = Input.mousePosition;
            Vector3 dragVector = dragEndPosition - dragStartPosition;

            // Calcular dirección 3D basada en la perspectiva de la cámara
            Vector3 throwDirection = mainCamera.transform.forward * dragVector.y + mainCamera.transform.right * dragVector.x;
            throwDirection.Normalize();

            // Calcular fuerza basada en la distancia del arrastre
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);
            float force = (dragDistance / maxDragDistance) * fuerzaMultiplicador * 1000;

            // Aplicar fuerza física
            rb.isKinematic = false;
            rb.AddForce(throwDirection * force, ForceMode.Impulse);

            isDragging = false;
            lastThrowTime = Time.time;
        }
    }

    void DrawDragLine()
    {
        Vector3 currentMousePos = Input.mousePosition;
        Vector3 worldStart = mainCamera.ScreenToWorldPoint(new Vector3(dragStartPosition.x, dragStartPosition.y, mainCamera.nearClipPlane));
        Vector3 worldEnd = mainCamera.ScreenToWorldPoint(new Vector3(currentMousePos.x, currentMousePos.y, mainCamera.nearClipPlane));

        Debug.DrawLine(worldStart, worldEnd, Color.red);
        Debug.DrawLine(transform.position, transform.position + (worldEnd - worldStart).normalized * 2, Color.green);
    }

    // Resetear posición (llamar desde otro script al colisionar con el suelo)
    public void ResetBall()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 1.5f, 0);
    }
}