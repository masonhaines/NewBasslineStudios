using System;
using System.Collections;

using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIControllerMinotaur : AIController
{
    public AttackComponentBeta AttackComponentBetaObj;
    private bool bHurt = false;
    protected override void Awake()
    {
        base.Awake(); 
        AttackComponentBetaObj = GetComponent<AttackComponentBeta>();
        AttackController = AttackComponentBetaObj;
        AttackController?.Initialize(myAnimator);
    }

    public override void PerceptionTargetLost(Transform target)
    {
        // never lose target
        bHasPerceivedTarget = true;
        detectedTargetTransform = target;
    }

    protected override void OnHitListener(Transform target, int damage)
    {
        base.OnHitListener(target, damage);
        bHurt = true;
        StartCoroutine(DontAnimateHurt());
    }

    protected override void FixedUpdate()
    {
        if (currentState == death)
        {
            
            return;
        }
        if (AttackComponentBetaObj.bIsDashing)
        {
            return;
        }else if (AttackController.bAttacking || bHurt)
        {
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
    
    protected IEnumerator DontAnimateHurt()
    {
        yield return new WaitForSeconds(4.0f);
        RevertColor();
    }
    
}
