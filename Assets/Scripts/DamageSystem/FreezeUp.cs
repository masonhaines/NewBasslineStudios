using System.Collections;
using UnityEngine;

public class FreezeUp : MonoBehaviour
{
    
    
    

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip soundOne;
    public Collider2D localCollider;
    public bool bFreeze = false;
    public float frozenTimer = 0;
    
    

    protected void Awake()
    {
        // sfxSource = GetComponent<AudioSource>(); 
        localCollider = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        
    }

    protected void Update()
    {
        

    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        else if (bFreeze)
        {
            Freeze(other);
        }
        // localCollider.enabled = false;
        
    }

    protected void Freeze(Collider2D other)
    {
        
        other.gameObject.GetComponent<PlayerController2D>().moveSpeed = other.gameObject.GetComponent<PlayerController2D>().InitMoveSpeed * .05f; // cut move speed in half

        SpriteRenderer spriteRenderer = other.gameObject.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color frozenColor = new Color(0.2f, 0.8f, 1.0f, 0.5f); 
            
            spriteRenderer.color = frozenColor;
            StartCoroutine(UnFreeze(other));
        }
    }

    private IEnumerator UnFreeze(Collider2D other)
    {
        yield return new WaitForSeconds(frozenTimer);
        
        SpriteRenderer spriteRenderer = other.gameObject.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color normalColor = new Color(1, 1, 1, 1f); 
            spriteRenderer.color = normalColor;
            float currentSpeed = other.gameObject.GetComponent<PlayerController2D>().moveSpeed;
            other.gameObject.GetComponent<PlayerController2D>().moveSpeed = other.gameObject.GetComponent<PlayerController2D>().InitMoveSpeed; // return move speed
        }
    }
}
