// using UnityEngine;
// using Combat; // this is the includable for the IDamageable

// public class HealthComponent : MonoBehaviour, IDamageable
// {
//     public event System.Action OnDeathCaller = delegate { };
//     public event System.Action<Transform> OnHitCaller = delegate { };
//     [SerializeField] private bool destroyOnDeath = true; // set FALSE on the Player only

//     [SerializeField] private int maxHealth;
//     [SerializeField] private float knockBackMultiplier = 1; // this is a multiplier for the knockback force applied when taking damage
//     private int currentHealth;
//     private KnockBack knockBack;

//     private void Awake() // Awake is called when an enabled script instance is being loaded.
//     {
//         knockBack = GetComponent<KnockBack>();
//         currentHealth = maxHealth;
//     }

//     public void Damage(int damageAmount, GameObject damageSource, float knockBackAmount, float knockBackLiftAmount)
//     {
//         currentHealth -= damageAmount;
//         OnHit(damageSource.transform);
//         knockBack.CreateKnockBack(damageSource.transform, knockBackAmount + knockBackMultiplier, knockBackLiftAmount);
//         if (currentHealth <= 0)
//         {
//             Death();
//         }
//     }
//     public void RestoreFullHealth()
//     {
//         currentHealth = maxHealth;
//     }

//     public bool GetIsKnockedBack()
//     {
//         return knockBack.bKnockedBack;
//     }

//     // private void Death()
//     // {
//     //     // call invoke so listener instance can take action. only listener should be the ai controller and player controller 
//     //     // OnDeathCaller?.Invoke(); // if not null invoke, rider recommended this null propogation as opposed to if null------- this is what needs to be set!
//     //     Destroy(gameObject);

//     //     Debug.Log("Death");
//     // }
//     private void Death()
//     {
//         // Notify listeners (RespawnManager will handle player respawn)
//         OnDeathCaller?.Invoke();

//         Debug.Log("Death");

//         if (destroyOnDeath)
//         {
//             Destroy(gameObject);   // keep for enemies
//         }
//         // else: do not destroy — player will be teleported & refilled by RespawnManager
//     }

//     private void OnHit(Transform hitTransform)
//     {
//         OnHitCaller?.Invoke(hitTransform);
//     }
// }


// // 0.2857143
// // 0.25


using System.Collections;
using UnityEngine;
using Combat; // this is the includable for the IDamageable

public class HealthComponent : MonoBehaviour, IDamageable
{
    public event System.Action OnDeathCaller = delegate { };
    public event System.Action<Transform> OnHitCaller = delegate { };

    // NEW: fired whenever we restore to full (e.g., on respawn)
    public event System.Action OnHealthRestored = delegate { };

    [SerializeField] private bool destroyOnDeath = true; // set FALSE on the Player only

    [SerializeField] private int maxHealth;
    [SerializeField] private float knockBackMultiplier = 1; // this is a multiplier for the knockback force applied when taking damage
    [SerializeField] private float TimeTillDestroy = 2.0f;
    private int currentHealth;
    private KnockBack knockBack;

    private void Awake() // Awake is called when an enabled script instance is being loaded.
    {
        knockBack = GetComponent<KnockBack>();
        currentHealth = maxHealth;
    }

    public void Damage(int damageAmount, GameObject damageSource, float knockBackAmount, float knockBackLiftAmount)
    {
        currentHealth -= damageAmount;
        OnHit(damageSource.transform);
        knockBack.CreateKnockBack(damageSource.transform, knockBackAmount + knockBackMultiplier, knockBackLiftAmount);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        OnHealthRestored?.Invoke(); // tell UI to refill hearts
    }

    public bool GetIsKnockedBack()
    {
        return knockBack.bKnockedBack;
    }

    private void Death()
    {
        // Notify listeners (RespawnManager will handle player respawn)
        OnDeathCaller?.Invoke();

        Debug.Log("Death");

        if (destroyOnDeath) // for enemies
        {
            // Destroy(gameObject, TimeTillDestroy);   // keep for enemies
        }
        // else: do not destroy — player will be teleported & refilled by RespawnManager
    }

    private void OnHit(Transform hitTransform)
    {
        OnHitCaller?.Invoke(hitTransform);
    }
}
