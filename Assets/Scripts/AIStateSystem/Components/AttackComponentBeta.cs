using System;
using System.Collections;
using UnityEngine;


public class AttackComponentBeta: MonoBehaviour, ICoreAttack
{
    public event System.Action AddToAttackCount = delegate { };
    
    private Animator animator;
    public Rigidbody2D attackerRigidBody;
    private AIController aiController;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float attackWaitTime = 3f;
    [SerializeField] private float dashScale = 1.0f;
    [SerializeField] private float dashTimeFrame = 1.0f;
    
    public bool bPrimaryAttackActive { get; set; } = false;
    public bool bAttacking { get; set; }
    public bool bIsDashing { get; set; } = false;
    private int attackCount = 0;
    private bool bTimerFinished = false;
    public void Initialize(Animator animatorRef)
    {
        aiController = GetComponentInParent<AIController>();
        animator = animatorRef;
        hitbox.enabled = false;
        bTimerFinished = true;
    }

    public void StartAttack()
    {
        if (!bTimerFinished || bAttacking) return; // if timer not finished return
        bTimerFinished = false;
        if (attackCount % 3 == 0)
        {
            AttackOne();
        }
        else
        {
            StartAttackTwo();
        }
    }

    public void AttackOne()
    {
        bAttacking = true;
        animator.SetBool("bAttackOne", true);
        attackCount++;
    }

    private IEnumerator AttackFinished()
    {
        yield return new WaitForSeconds(attackWaitTime);
        bTimerFinished = true;
    }
    
    public void StartAttackTwo()
    {
        bAttacking = true;
        animator.SetBool("bAttackTwo", true);
        attackCount++;
    }

    public void EndAttackTwo()
    {
        animator.SetBool("bAttackTwo", false);
    }
    
    public void EndAttackOne()
    {
        animator.SetBool("bAttackOne", false);
    }
    
    public void DamageEnabled()
    {
        hitbox.enabled = true;
    }

    public void DamageDisabled()
    {
        hitbox.enabled = false;
        StartCoroutine(AttackFinished());
        bAttacking = false;
    }
    
    public void DamageDisabledOnHit()
    {
        hitbox.enabled = false;
        bAttacking = false;
    }

    
    public void StartDash()
    {
        // Debug.Log("Dash is activated");
        bIsDashing = true;
        StartCoroutine(Dash());
    }
    
    // this needs to be called as a trigger inside of the anim, just like the collision enable and disable
    private IEnumerator Dash()
    {
        // float OG_gravity = attackerRigidBody.gravityScale;
        // attackerRigidBody.gravityScale = 0f;
        // attackerRigidBody.linearVelocity = new Vector2(transform.localScale.x * dashScale, 0f);
        // start emitter here if you want that kinda thing maybe kinda sorta, it is too late to be doing this lol
        attackerRigidBody.AddForce(
            new Vector2(aiController.facingDirection * dashScale, 0f),
            ForceMode2D.Impulse
        );
        yield return new WaitForSeconds(dashTimeFrame);
        // turn of the emitter at this point
        // attackerRigidBody.gravityScale = OG_gravity;
        bIsDashing = false;
        
        // Debug.Log("Dash is activated inside of the coroutine");
    }
    
    
    
}
