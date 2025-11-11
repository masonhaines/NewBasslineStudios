using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackTime = 0.5f;
    private Rigidbody2D knockBackRigidbody2D;
    public bool bKnockedBack = false;

    private void Awake()
    {
        knockBackRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void CreateKnockBack(Transform damageSource, float knockBackAmount, float knockBackLiftAmount)
    {
        if (bKnockedBack) return;

        // Debug.Log("Trying to knockBack");
        //---------------------// Victim of attack //
        Vector2 difference = (transform.position - damageSource.position).normalized;
        // Debug.Log("the difference:" + difference);
        // if player jumps into hazard from belo
        if (difference.y < 0)  
        {
            // difference.y += knockBackLiftAmount * 10.0f; 
            // difference.x *= -1.2f;
            difference.y = -knockBackAmount;            
        }
        // else
        // {
        //     difference.y += knockBackLiftAmount; 
        // }
        
        bKnockedBack = true;
        difference.y = knockBackLiftAmount;
        
        // stop any existing momentum before applying force - chat - fix on double knock back force occurring 
        knockBackRigidbody2D.linearVelocity = Vector2.zero;
        knockBackRigidbody2D.AddForce(difference * knockBackAmount * knockBackRigidbody2D.mass, ForceMode2D.Impulse);

        StartCoroutine(KnockBackCoroutine());
        
    }

    private IEnumerator KnockBackCoroutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        bKnockedBack = false;
    }
}
