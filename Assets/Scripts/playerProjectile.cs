using UnityEngine;
using Combat; // for IDamageable interface

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 12f;
    public float lifetime = 3f;
    public int damage = 1;
    public float knockbackAmount = 5f;
    public float knockbackLift = 2f;
    public LayerMask hitLayers; // assign to "Enemies" or whatever you want to hit

    [Header("Optional")]
    public Rigidbody2D rb;

    private Vector2 launchDirection;
    private bool initialized = false;
    private GameObject owner;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, GameObject owner, float inheritedVelocity = 0f)
    {
        this.owner = owner;
        launchDirection = direction.normalized;
        rb.linearVelocity = launchDirection * speed + new Vector2(inheritedVelocity, 0f);
        initialized = true;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore hitting the player (the shooter)
        if (collision.gameObject == owner) return;

        // Make sure the collision object is on a hittable layer
        if (((1 << collision.gameObject.layer) & hitLayers) == 0)
            return;

        // Try to damage anything with a HealthComponent (implements IDamageable)
        HealthComponent targetHealth = collision.GetComponent<HealthComponent>();
        if (targetHealth != null)
        {
            targetHealth.Damage(damage, owner, knockbackAmount, knockbackLift);
        }

        // Optionally: add particle or sound effect here

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!initialized)
            return;

        // maintain forward velocity (helps if physics drag slows it)
        rb.linearVelocity = launchDirection * speed;
    }
}
