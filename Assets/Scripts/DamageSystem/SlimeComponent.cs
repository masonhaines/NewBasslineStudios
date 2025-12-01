using System.Collections;
using UnityEngine;

public class SlimeComponent : MonoBehaviour
{
        
    

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip soundOne;
    [SerializeField] private float soundCooldownSeconds = 1f;
    public Collider2D localCollider;
    public bool bSlow = false;
    private bool soundOnCooldown = false;

    protected void Awake()
    {
        sfxSource = GetComponent<AudioSource>(); 
        localCollider = GetComponent<Collider2D>();
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (bSlow)
        {
            Slow(other);

            if (!soundOnCooldown)
            {
                StartCoroutine(SoundCooldownRoutine());
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.gameObject.GetComponent<PlayerController2D>().moveSpeed =
            other.gameObject.GetComponent<PlayerController2D>().InitMoveSpeed;
    }
    
    private IEnumerator SoundCooldownRoutine()
    {
        soundOnCooldown = true;

        if (sfxSource)
        {
            sfxSource.PlayOneShot(soundOne);
        }
        
        yield return new WaitForSeconds(soundCooldownSeconds);

        soundOnCooldown = false;
    }

    protected void Slow(Collider2D other)
    {
        float currentSpeed = other.gameObject.GetComponent<PlayerController2D>().moveSpeed;
        other.gameObject.GetComponent<PlayerController2D>().moveSpeed = currentSpeed * .25f; // cut move speed in half
    }
}
