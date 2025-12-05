using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIControllerMinotaur : AIController
{
    public AttackComponentBeta AttackComponentBetaObj;
    private bool bHurt = false;
    public bool jumping;

    private float g, b = 1.0f;
    private float colorDecrement = 1.0f;
    
    
    protected override void Awake()
    {
        base.Awake(); 
        AttackComponentBetaObj = GetComponent<AttackComponentBeta>();
        AttackController = AttackComponentBetaObj;
        AttackController?.Initialize(myAnimator);
        b = 1.0f;
        g = 1.0f;
        
    }

    public override void PerceptionTargetFound(Transform target)
    {
        base.PerceptionTargetFound(target);
        colorDecrement = healthComponentObject.currentHealth;
    }

    public override void PerceptionTargetLost(Transform target)
    {
        // never lose target
        bHasPerceivedTarget = true;
        detectedTargetTransform = target;
    }

    

    protected override void FixedUpdate()
    {
        if (currentState == death || jumping)
        {
            return;
        }
        if (AttackComponentBetaObj.bIsDashing || jumping)
        {
            return;
        }
        else if (AttackController.bAttacking || bHurt)
        {
            // Debug.Log("check?");
            MovementController.StopMovement();
            return;
        }
        
        if ((!bInRangeToAttack && bHasPerceivedTarget) && !healthComponentObject.GetIsKnockedBack())
        {
            if (detectedTargetTransform) // safety guard
            {
                if (currentState != chase)
                {
                    setNewState(chase);
                }
                
                chaseComponentObject.UpdateChaseLocation(detectedTargetTransform);
                MovementController.OnTick();
            }
        }
        else if (!AttackController.bAttacking)
        {
            MovementController.OnTick();
        }
        else
        {
            myAnimator.SetTrigger("tNotMoving");
        }
    }

    protected override IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.2f);
        RevertColor();
    }
    
    protected override void OnHitListener(Transform target, int damage)
    {
        base.OnHitListener(target, damage);
        bHurt = true;
        UpdateColorFromDamageTaken();
        StartCoroutine(DontAnimateHurt());
        StartCoroutine(NotHurtTimer());
    }
    
    protected IEnumerator DontAnimateHurt()
    {
        yield return new WaitForSeconds(4.0f);
        RevertColor();
    }
    
    protected IEnumerator NotHurtTimer()
    {
        yield return new WaitForSeconds(2.1f);
        bHurt = false;
    }

    private void UpdateColorFromDamageTaken()
    {
        if (b > 0f || g > 0f)
        {
            g = Mathf.Max(0f, g - 1 / colorDecrement);
            b = Mathf.Max(0f, b - 1 / colorDecrement);
        }
        
        originalColor = new Color(1f, g, b, 1f);
        
    }
    
}
