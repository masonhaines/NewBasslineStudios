// using UnityEngine;

// public class ParallaxLayer : MonoBehaviour
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

[DisallowMultipleComponent]
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField, Range(0f,1f)] float strength = 0.3f; // smaller = farther
    Transform cam;
    Vector3 lastCamPos;

    void Start()
    {
        var main = Camera.main;
        if (main != null)
        {
            cam = main.transform;
            lastCamPos = cam.position;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;
        var delta = cam.position - lastCamPos;
        // Horizontal-only parallax; change to new Vector3(delta.x * strength, delta.y * strength, 0)
        // if you also want vertical.
        transform.position += new Vector3(delta.x * strength, 0f, 0f);
        lastCamPos = cam.position;
    }
}
