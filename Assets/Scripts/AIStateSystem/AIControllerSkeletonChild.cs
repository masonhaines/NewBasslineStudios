using System;
using System.Collections;

using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIControllerSkeletonChild : AIController
{
    protected override void Awake()
    {

       
        base.Awake(); 
        attackComponentObject = GetComponent<AttackComponent>();
        AttackController = attackComponentObject;
        AttackController?.Initialize(myAnimator);
    }
    protected override void FixedUpdate()
    {
        
        Debug.Log(attackComponentObject.bPrimaryAttackActive);
        
        if (currentState == death)
        {
            
            return;
        }
        if (attackComponentObject.bIsDashing)
        {
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
        else if (currentState == patrol && !bIsAttacking)
        {
            
            MovementController.OnTick();
        }
    
    }
    
    // called everytime the attack trigger is pulled
    protected override void OnAttackCounting()
    {
        localAttackCounter++;
        // Debug.Log($"{name} attack count triggered");
        if (localAttackCounter >= maxAttacksBeforeReset )
        {
            // Debug.Log("running attack counter logic");
            
            // attackComponentObject.bAttackFinished = true;
            // attackComponentObject.StartAttackTwo();
            attackComponentObject.bAttackTwo = true;
            localAttackCounter = 0;
        }

       
    }
    
}
