using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothing = 5f;

    [Header("Isometric Camera")]
    public float distance = 10f;
    public float height = 8f;
    public float angle = 45f;

    void Start()
    {
        SetCameraPosition();
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        SetCameraPosition();
    }

    void SetCameraPosition()
    {
        Vector3 offset = new Vector3(0f, height, -distance);

        // Rotate offset around Y axis
        offset = Quaternion.Euler(0f, angle, 0f) * offset;

        Vector3 targetCamPos = target.position + offset;

        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

        // Look at Player
        transform.LookAt(target);
    }
}