using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    private Vector3 offset;

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        // Calculate the camera's target position.
        Vector3 targetCamPos = target.position + offset;

        // Smoothly move the camera to the target position.
        transform.position = Vector3.Lerp(
            transform.position,
            targetCamPos,
            smoothing * Time.deltaTime
        );
    }
}