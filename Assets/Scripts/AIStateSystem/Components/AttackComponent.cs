using System;
using System.Collections;
using UnityEngine;


public class AttackComponent: MonoBehaviour, ICoreAttack
{
    public event System.Action AddToAttackCount = delegate { };
    
    private Animator animator;
    public Rigidbody2D attackerRigidBody;
    private AIController aiController;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float attackWaitTime = 0f;
    [SerializeField] private bool bNeedsAttackCounter = false;
    [SerializeField] private float dashScale = 1.0f;
    [SerializeField] private float dashTimeFrame = 1.0f;
    [SerializeField] private string attackAnimStateName;
    
    public bool bAttackFinished { get; set; } = true;
    public bool bAttackTwoFinished { get; set; } = false;
    private bool bInitialized = false;
    public bool bIsDashing { get; set; } = false;

    public void Initialize(Animator animatorRef)
    {
        // attackerRigidBody = GetComponentInParent<Rigidbody2D>();
        aiController = GetComponentInParent<AIController>();
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
    public void StartAttackTwo()
    {
        bAttackTwoFinished = false;
        animator.SetBool("bAttackTypeTwo", true);
    }

    public void EndAttackTwo()
    {
        bAttackTwoFinished = true;
        animator.SetBool("bAttackTypeTwo", false);
    }

    
    public void StartDash()
    {
        Debug.Log("Dash is activated");
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
            new Vector2(aiController.MovementController.facingDirection * dashScale, 0f),
            ForceMode2D.Impulse
        );
        yield return new WaitForSeconds(dashTimeFrame);
        // turn of the emitter at this point
        // attackerRigidBody.gravityScale = OG_gravity;
        bIsDashing = false;
        
        Debug.Log("Dash is activated inside of the coroutine");
    }
    
}
