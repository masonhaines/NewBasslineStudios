using System;
using System.Collections;
using Combat;
using UnityEngine;

public class ImpactDamageComp : MonoBehaviour
{
    
    [SerializeField] private int damageAmount;
    [SerializeField] private bool bProjectile;
    [SerializeField] private float knockBackAmount = 3;
    [SerializeField] private float knockBackLiftAmount;
    [SerializeField] public bool bIsEnabled = false;
    [SerializeField] private float timeBetweenAttacks = 0.2f;
    [SerializeField] private bool debugging;
    [SerializeField] private bool bDestroyOnImpact;
    
    private int originalDamage;


    private float timeSinceLastAttack;
    private bool canAttack = true;
    private GameObject damageSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() // Awake is called when an enabled script instance is being loaded.
    {
        damageSource = transform.root.gameObject;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= timeBetweenAttacks)
        {
            canAttack = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (debugging)
        {
            Debug.Log(other.gameObject.name);
        }
        if (!bIsEnabled) return;
        
        if (other.gameObject == damageSource || (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Enemy")) ) {
            return; // dont damage self or friends 
        }
        
        var damageable = other.GetComponent<IDamageable>(); // recommended type var on rider?
        if (canAttack && damageable != null) 
        {
            damageable.Damage(damageAmount, damageSource, knockBackAmount, knockBackLiftAmount);
            canAttack = false;
            timeSinceLastAttack = 0;
        }
        if (bDestroyOnImpact) Destroy(gameObject, 0.0f);
    }
    
    public void IncreaseDamage(int amount)
    {
        damageAmount += amount;
        StartCoroutine(RevertDamage());
    }

    private IEnumerator RevertDamage()
    {
        yield return new WaitForSeconds(10.0f);
        damageAmount = originalDamage;
    }
    
    
}
