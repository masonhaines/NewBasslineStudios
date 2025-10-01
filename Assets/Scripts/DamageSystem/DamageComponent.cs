using System;
using Combat;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    
    [SerializeField] private int damageAmount;
    [SerializeField] private bool bProjectile;
    [SerializeField] private float knockBackAmount = 3;
    [SerializeField] private float knockBackLiftAmount;


    private float TimeSinceLastAttack;
    private bool canAttack = true;
    
    private GameObject damageSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() // Awake is called when an enabled script instance is being loaded.
    {
        damageSource = transform.root.gameObject;
    }

    private void Update()
    {
        TimeSinceLastAttack += Time.deltaTime;
        if (TimeSinceLastAttack >= 0.2f)
        {
            canAttack = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject == damageSource) {
            return; // dont damage self
        }
        var damageable = other.GetComponent<IDamageable>(); // recommended type var on rider?
        if (canAttack && damageable != null) 
        // if (damageable != null)
        {
            damageable.Damage(damageAmount, damageSource, knockBackAmount, knockBackLiftAmount);
            canAttack = false;
            TimeSinceLastAttack = 0;
        }
    }
}
