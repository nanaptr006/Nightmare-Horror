using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private bool autoFindTarget = true;

    [Header("Follow")]
    [SerializeField] private float targetHeight = 1.5f;
    [SerializeField] private float followSmoothTime = 0.08f;

    [Header("Orbit")]
    [SerializeField] private float distance = 5.5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 9f;
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float pitch = 15f;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Collision")]
    [SerializeField] private LayerMask collisionMask = -1;
    [SerializeField] private float cameraRadius = 0.25f;
    [SerializeField] private float collisionOffset = 0.15f;

    private Vector3 followVelocity;

    private void Start()
    {
        if (autoFindTarget && target == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target == null)
        {
            Debug.LogWarning(
                "ThirdPersonCamera: No Player found."
            );

            enabled = false;
            return;
        }

        distance = Mathf.Clamp(
            distance,
            minDistance,
            maxDistance
        );
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        RotateCamera();
        ZoomCamera();
        FollowPlayer();
    }

    // =========================
    // Rotate Camera
    // =========================

    private void RotateCamera()
    {
        if (Mouse.current == null)
            return;

        // Hold Right Mouse Button
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta =
                Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * mouseSensitivity * 0.01f;
            pitch -= mouseDelta.y * mouseSensitivity * 0.01f;

            pitch = Mathf.Clamp(
                pitch,
                minPitch,
                maxPitch
            );
        }
    }

    // =========================
    // Zoom
    // =========================

    private void ZoomCamera()
    {
        if (Mouse.current == null)
            return;

        float scroll =
            Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            distance -=
                scroll * 0.01f * zoomSpeed;

            distance = Mathf.Clamp(
                distance,
                minDistance,
                maxDistance
            );
        }
    }

    // =========================
    // Follow Player
    // =========================

    private void FollowPlayer()
    {
        // Position camera around Player
        Vector3 targetPosition =
            target.position +
            Vector3.up * targetHeight;

        Quaternion orbitRotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 direction =
            orbitRotation * Vector3.back;

        Vector3 desiredPosition =
            targetPosition +
            direction * distance;

        // Prevent camera from going through walls
        Vector3 finalPosition =
            CheckCollision(
                targetPosition,
                desiredPosition
            );

        // Smooth follow
        transform.position =
            Vector3.SmoothDamp(
                transform.position,
                finalPosition,
                ref followVelocity,
                followSmoothTime
            );

        // Look at Player
        Vector3 lookDirection =
            targetPosition -
            transform.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    lookDirection,
                    Vector3.up
                );
        }
    }

    // =========================
    // Camera Collision
    // =========================

    private Vector3 CheckCollision(
        Vector3 targetPosition,
        Vector3 desiredPosition)
    {
        Vector3 direction =
            desiredPosition -
            targetPosition;

        float distanceToCamera =
            direction.magnitude;

        if (distanceToCamera <= 0.01f)
            return desiredPosition;

        direction.Normalize();

        RaycastHit hit;

        if (Physics.SphereCast(
            targetPosition,
            cameraRadius,
            direction,
            out hit,
            distanceToCamera,
            collisionMask,
            QueryTriggerInteraction.Ignore))
        {
            float safeDistance =
                hit.distance -
                collisionOffset;

            safeDistance =
                Mathf.Clamp(
                    safeDistance,
                    minDistance,
                    distanceToCamera
                );

            return targetPosition +
                   direction * safeDistance;
        }

        return desiredPosition;
    }
}
