using System;
using System.Collections;
using UnityEngine;


public class AttackComponent: MonoBehaviour, ICoreAttack
{
    public event System.Action AddToAttackCount = delegate { };
    
    private Animator animator;
    public Rigidbody2D attackerRigidBody;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float attackWaitTime = 0f;
    [SerializeField] private bool bNeedsAttackCounter = false;
    [SerializeField] private float dashScale = 1.0f;
    [SerializeField] private float dashTimeFrame = 1.0f;
    
    public bool bAttackFinished { get; set; } = true;
    private bool bInitialized = false;

    public void Initialize(Animator animatorRef)
    {
        attackerRigidBody = GetComponent<Rigidbody2D>();
        animator = animatorRef;
        DamageDisabled();
        bInitialized = true;
    }

    public void StartAttack()
    {
        if (!bAttackFinished)
        {
            return;
        }
        bAttackFinished = false;
        animator.SetTrigger("tCanAttackTarget");
    }

    public void DamageEnabled()
    {
        hitbox.enabled = true;
    }

    public void DamageDisabled()
    {
        hitbox.enabled = false;
        StartCoroutine(AttackFinished());
        
        if (bNeedsAttackCounter && bInitialized)
        {
            AddToAttackCount.Invoke();
        }

        
    }

    private IEnumerator AttackFinished()
    {
        yield return new WaitForSeconds(attackWaitTime);
        bAttackFinished = true;
    }
    
    // this needs to be called after the attack two animtion to switch attack two off or attacks will stop
    public void SwitchToAttackTwo()  
    {
        if (animator.GetBool("the namof that bool for attacj two that dont remember"))
        {
            animator.SetBool("i dont remember the name of the bool so change this later", true);
        }
        else
        {
            animator.SetBool("i dont remember the name of the bool so change this later", false);
        }
    }
    
    
    // this needs to be called as a trigger inside of the anim, just like the collision enable and disable
    private IEnumerator Dash()
    {
        float OG_gravity = attackerRigidBody.gravityScale;
        attackerRigidBody.gravityScale = 0f;
        attackerRigidBody.linearVelocity = new Vector2(transform.localScale.x * dashScale, 0f);
        // start emitter here if you want that kinda thing maybe kinda sorta, it is too late to be doing this lol
        yield return new WaitForSeconds(dashTimeFrame);
        // turn of the emitter at this point
        attackerRigidBody.gravityScale = OG_gravity;
    }
    
}
