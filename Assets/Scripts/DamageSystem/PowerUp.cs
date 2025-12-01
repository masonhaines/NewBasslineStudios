using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private SpriteRenderer sprite;
    private ImpactDamageComp damageComponent;
    
    [SerializeField] private Color friendlyColor;
    [SerializeField] private Color foeColor;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip soundOne;
    public Collider2D localCollider;

    
    public bool bHealthUp = false;
    public bool bPowerUp = false;
    public bool bPowerDown = false;
    public bool bFriendly = true;
    public bool toggleFriendly = false;
    public float friendOrFoeTimer = 0;
    public int adjustHealth = 1;
    public int adjustPower = 1;
    

    protected void Awake()
    {
        damageComponent = GetComponent<ImpactDamageComp>();
        sprite = GetComponent<SpriteRenderer>();
        sfxSource = GetComponent<AudioSource>(); 
        localCollider = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        
        if (toggleFriendly)
        {
            damageComponent.enabled = true;
            StartCoroutine(FriendOrFoe());
        }
        else
        {
            damageComponent.enabled = false;
        }
    }

    protected void Update()
    {
        if (bFriendly)
        {
            // damageComponent.enabled = false; 
            damageComponent.bIsEnabled = false;
            sprite.color = friendlyColor;

        }
        else
        {
            damageComponent.bIsEnabled = true; 
            sprite.color = foeColor;

        }

    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (bFriendly && bHealthUp)
        {
            // Debug.Log("Friendly " + other.gameObject.name);
            sfxSource.PlayOneShot(soundOne);
            other.GetComponent<HealthComponent>().AddHealth(adjustHealth);
        }
        else if (bFriendly && bPowerUp)
        {
            sfxSource.PlayOneShot(soundOne);
            other.GetComponentInChildren<DamageComponent>().IncreaseDamage(adjustPower);
        }
        // else if (bPowerDown && !bFriendly)
        // {
        //     other.GetComponent<ImpactDamageComp>().IncreaseDamage(-adjustPower);
        // }
        localCollider.enabled = false;
        sprite.enabled = false;
        Destroy(gameObject, 60.0f);
    }

    private IEnumerator FriendOrFoe()
    {
        while (true)
        {
            yield return new WaitForSeconds(friendOrFoeTimer);
            bFriendly = !bFriendly;
        }
    }


}
