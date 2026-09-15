using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 6f; // The speed that the player will move at.
                             // The vector to store the direction of the player's movement
    private Vector3 movement;
    // Reference to the player's rigidbody.
    private Rigidbody playerRigidbody;
    // A layer mask so that a ray can be cast just at the floor .
    private int floorMask;
    // The length of the ray from the camera into the sceen
    private float camRayLength = 100f;
    private Animator anim; // Reference to the animator component.
    void Awake()// Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
        // Set up references.
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        // Move the player around the scene.
        Move(h, v);
        // Turn the player to face the mouse cursor.
        Turning();
        Animating(h, v);
    }
    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);
        // Normalise the movement vector and make it proportional to the speed per second
        movement = movement.normalized * speed * Time.deltaTime;
        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }
    RaycastHit floorHit;
    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit RaycastHit floorHit;
        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the ray hits
            Vector3 playerToMouse = floorHit.point - transform.position;
            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;
            // Create a quaternion (rotation) based on the vector from
            // the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        anim = GetComponent<Animator>();
        // Create a boolean that is true if either of the input axes is non-zero.

        bool IsWalking = h != 0f || v != 0f;
        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", IsWalking);
    }
}
