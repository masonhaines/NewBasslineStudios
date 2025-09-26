using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Drag Player here
    public Vector3 offset;     //for adjusting position
    public float smoothSpeed = 0.125f; // Smooth movement

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
        target.position.x + offset.x,    // follow player horizontally
        transform.position.y,           // lock Y (don’t follow jumps)
        target.position.z + offset.z    // keep camera depth
        );

    //Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

    //Apply to camera
        transform.position = smoothedPosition;
    }
}
