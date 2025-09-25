using UnityEngine;
using Combat; // this is the includable for the IDamageable

public class HealthComponent : MonoBehaviour, IDamageable
{

    [SerializeField] private int health;
    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake() // Awake is called when an enabled script instance is being loaded.
    {
        currentHealth = health;
    }
    
    // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    // This function can be a coroutine.
    void Start()
    {
        
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"{gameObject.name} is now {health} after damage from opponent");
        Debug.Log("current health: " + health); // using + for this is so JS


        if (health <= 0)
        {
            Death();
            Debug.Log("Object is dead");
        }

    }

    private void Death()
    {
        Destroy(gameObject); // this really should be pooled, maybe with a pooling interface and a global script to destroy at some points in game 
        Debug.Log("Object is dead");
    }
}
