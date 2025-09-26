using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Drag Player here
    public Vector3 offset;     //for adjusting position
    public float smoothSpeed = 0.125f; // Smooth movement

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
