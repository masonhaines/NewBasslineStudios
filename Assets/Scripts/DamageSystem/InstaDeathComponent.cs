
using System.Collections;
using Combat;
using UnityEngine;

public class InstaDeathComponent : MonoBehaviour
{
    [SerializeField] private bool bIsEnabled = false;
    [SerializeField] private bool debugging;
    private GameObject damageSource;

    void Awake()
    {
        damageSource = transform.root.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!bIsEnabled) return;
        if (other.gameObject == damageSource || (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Enemy")) ) {
            return; // dont damage self or friends 
        }
        
        float tempGravity = other.GetComponent<Rigidbody2D>().gravityScale;
        other.gameObject.GetComponent<Rigidbody2D>().gravityScale = tempGravity * .15f;
        
        
        var damageable = other.GetComponent<IDamageable>(); // recommended type var on rider?
        if (damageable != null)
        {
            damageable.Damage(100, damageSource, 0, 0);
        }

        if (debugging)
        {
            Debug.Log(other.gameObject.name);
        }
    }
    
}