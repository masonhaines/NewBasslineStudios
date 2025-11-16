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
    [SerializeField] private float attackWaitTime = 3f;
    [SerializeField] private bool bNeedsAttackCounter = false;
    [SerializeField] private float dashScale = 1.0f;
    [SerializeField] private float dashTimeFrame = 1.0f;
    // [SerializeField] private string attackAnimStateName;
    
    public bool bPrimaryAttackActive { get; set; } = false;
    public bool bAttacking { get; set; }
    public bool bSecondaryAttackActive { get; set; } = false;
    private bool bInitialized = false;
    public bool bIsDashing { get; set; } = false;
    
    public bool bAttackTwo;
    public bool onlySecondaryAttack = false;
    

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
        if (bAttacking) return;
        if (bPrimaryAttackActive) return;
        
        
        // Debug.Log("startattack being called");
        if (!bAttackTwo)
        {
            // Debug.Log("inside A1");

            AttackOne();
        }
        else
        {
           
            // Debug.Log("inside A2");

            StartAttackTwo();
        }
    }

    public void AttackOne()
    {
        if (bPrimaryAttackActive)
        {
            return;
        }
        // Debug.Log(">>> AttackOne START");   
        bPrimaryAttackActive = true;
        bAttacking = true;

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
        bAttacking = false;

        if (bNeedsAttackCounter && bInitialized)
        {
            AddToAttackCount?.Invoke();
        }
    }
    
    public void DamageDisabledOnHit()
    {
        hitbox.enabled = false;
        bAttacking = false;
    }
    
    
    // private float nextAttackAllowedTime = 0f;

    private IEnumerator AttackFinished()
    {
        // Debug.Log($"[{name}] AttackFinished START at {Time.time}, wait={attackWaitTime}");
        yield return new WaitForSeconds(attackWaitTime);
        bPrimaryAttackActive = false;
        // bAttacking = false;

        // bAttackTwo = onlySecondaryAttack;
        // Debug.Log($"[{name}] AttackFinished END at {Time.time}, primaryActive={bPrimaryAttackActive}");
        
    }

    
    // this needs to be called after the attack two animtion to switch attack two off or attacks will stop
    public void StartAttackTwo()
    {
        

        if (bAttacking)
        {
            // Debug.Log("I am being called but returning");
            return;
        }
        if (bPrimaryAttackActive) return;
        // Debug.Log("attack two being called ");

        
        
        bPrimaryAttackActive = true;
        bAttacking = true;
        bSecondaryAttackActive = false;
        animator.SetBool("bAttackTypeTwo", true);
        animator.SetTrigger("tCanAttackTarget");
    }

    public void EndAttackTwo()
    {
        
        bSecondaryAttackActive = false;
        bAttacking = false;

        animator.SetBool("bAttackTypeTwo", false);
        bAttackTwo = onlySecondaryAttack;
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
