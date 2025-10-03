// using UnityEngine;

// public class CameraFollow2d : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform target;                 // drag your Player here
    [SerializeField] Vector3 offset = new Vector3(0, 15, -20);
    [SerializeField] float smooth = 5f;                // 0 = snap, 5-10 = smooth

    void Start() {
        if (!target) {                                 // fallback if you forget to assign
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
    }

    void LateUpdate() {
        if (!target) return;
        var desired = target.position + offset;
        desired.z = offset.z;                          // keep fixed Z
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
}
