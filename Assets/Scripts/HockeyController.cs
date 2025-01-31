using UnityEngine;

public class HockeyController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody rb;
    private bool isDragging = false;
    private int activeTouchId = -1; // Store the touch ID associated with this controller
    private Vector3 targetPosition; // Store the target position to be used in FixedUpdate

    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }

    private void Update()
    {
        // Loop through all touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && activeTouchId == -1)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the ray hit the collider attached to this Rigidbody
                    if (hit.collider.attachedRigidbody == rb)
                    {
                        // The touch started on top of this Rigidbody
                        isDragging = true;
                        activeTouchId = touch.fingerId; // Store the touch ID
                    }
                }
            }

            // Handle dragging
            if (isDragging && touch.fingerId == activeTouchId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        // Store the target position to apply in FixedUpdate
                        targetPosition = new Vector3(hit.point.x, rb.position.y, hit.point.z);
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                    activeTouchId = -1; // Reset the touch ID when the touch ends
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 targetVelocity = (targetPosition - rb.position) * speed * Time.fixedDeltaTime;

            Vector3 velocityChange = targetVelocity - rb.linearVelocity;
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z); // Ignore Y axis

            velocityChange = Vector3.ClampMagnitude(velocityChange, speed);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
