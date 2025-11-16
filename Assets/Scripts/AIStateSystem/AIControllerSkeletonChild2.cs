using System;
using System.Collections;

using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIControllerSkeletonChild2 : AIController
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
    
    // public bool onlyAttackTwo;
    // protected override void OnAttackCounting()
    // {
    //     if (onlyAttackTwo)
    //     {
    //         stopMovementForAttackAnimation = false;
    //         attackComponentObject.StartAttackTwo();
    //     }
    //     else
    //     {
    //         localAttackCounter++;
    //         // Debug.Log($"{name} attack count triggered");
    //         if (localAttackCounter > maxAttacksBeforeReset)
    //         {
    //             stopMovementForAttackAnimation = false;
    //             attackComponentObject.StartAttackTwo();
    //             localAttackCounter = 0;
    //         }
    //
    //         stopMovementForAttackAnimation = true;
    //     }
    // }
    
}
