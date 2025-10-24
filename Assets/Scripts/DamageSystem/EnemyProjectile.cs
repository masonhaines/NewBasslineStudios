
using UnityEngine;

public class EnemyProjectile: MonoBehaviour
{

    public GameObject player;
    private Rigidbody projectileRigidbody;
    [SerializeField] public float force;
    [SerializeField] public float angleOffset;
    
    
    void Start()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        Vector3 projectileDirection = player.transform.position - transform.position;
        projectileRigidbody.linearVelocity = new Vector2(projectileDirection.x, projectileDirection.y).normalized * force;
        
        float rotationAngle = Mathf.Atan2(-projectileDirection.y, -projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle + angleOffset);
    }

    void Update()
    {
        
    }
}
