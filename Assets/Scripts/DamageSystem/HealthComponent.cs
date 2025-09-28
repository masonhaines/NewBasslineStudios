using UnityEngine;
using Combat; // this is the includable for the IDamageable

public class HealthComponent : MonoBehaviour, IDamageable
{
    public event System.Action OnDeathCaller = delegate { };  
    public event System.Action<Transform> OnHitCaller = delegate { };

    [SerializeField] private int maxHealth;
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
        knockBack.CreateKnockBack(damageSource.transform, knockBackAmount, knockBackLiftAmount);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public bool GetIsKnockedBack()
    {
        return knockBack.bKnockedBack;
    }

    private void Death()
    {
        // call invoke so listener instance can take action. only listener should be the ai controller and player controller 
        OnDeathCaller?.Invoke(); // if not null invoke, rider recommended this null propogation as opposed to if null
        Debug.Log("Death");
    }

    private void OnHit(Transform hitTransform)
    {
        OnHitCaller?.Invoke(hitTransform);
    }
}
// 0.2857143
// 0.25