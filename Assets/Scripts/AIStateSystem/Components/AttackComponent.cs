using System;
using System.Collections;
using UnityEngine;


public class AttackComponent: MonoBehaviour
{
    public event System.Action AddToAttackCount = delegate { };
    
    private Animator animator;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float attackWaitTime = 0f;
    [SerializeField] private bool bNeedsAttackCounter = false;
    
    public bool bAttackFinished;
    private bool bInitialized = false;
    
    
    public void Initialize(Animator animatorRef)
    {
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
        animator.SetTrigger("tCanAttackTarget");
    }

    public void DamageEnabled()
    {
        bAttackFinished = false;
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
    
}
