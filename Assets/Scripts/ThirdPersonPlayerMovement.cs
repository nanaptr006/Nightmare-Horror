using UnityEngine;

public class ThirdPersonPlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float rotationSpeed = 10f;

    private Vector3 movement;
    private Rigidbody playerRigidbody;
    private Animator anim;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        RotateTowardsMouse();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        // Ignore camera vertical angle
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Camera-relative movement
        movement = forward * v + right * h;

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Move Player
        Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;

        playerRigidbody.MovePosition(newPosition);
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create an invisible horizontal plane at player's height
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(distance);

            Vector3 direction = mouseWorldPosition - transform.position;

            // Ignore Y axis
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                Quaternion smoothRotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                playerRigidbody.MoveRotation(smoothRotation);
            }
        }
    }

    void Animating(float h, float v)
    {
        bool isWalking =
            h != 0f ||
            v != 0f;

        if (anim != null)
        {
            anim.SetBool("IsWalking",isWalking);
        }
    }
}