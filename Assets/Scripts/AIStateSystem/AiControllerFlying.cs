using System;
using System.Collections;

using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AiControllerFlying : AIController
{
    public ProjectileComponent projectileComponentObject;
    protected override void Awake()
    {
        detectedTargetTransform = null;
        MovementController = GetComponent<AiMovementComponent>();
        patrolComponentObject = GetComponent<PatrolComponent>();
        chaseComponentObject = GetComponent<ChaseComponent>();
        healthComponentObject = GetComponent<HealthComponent>();
        
        projectileComponentObject = GetComponentInChildren<ProjectileComponent>();
        AttackController = GetComponentInChildren<ICoreAttack>();
        AttackController = projectileComponentObject;
        myAnimator = GetComponentInChildren<Animator>(); // this is because the animator is in the sprite child object of the enemy prefab 
        
        // enemyRigidBody = GetComponent<Rigidbody2D>();
        healthComponentObject.OnDeathCaller += OnDeathListener;
        healthComponentObject.OnHitCaller += OnHitListener;
        
        AttackController?.Initialize(myAnimator); // if the ai controller has a ref to, call initialize 
    }

    protected override void Update()
    {
        currentState.PollPerception();

        // if (bHasPerceivedTarget && detectedTargetTransform is not null)
        // {
        //     
        //     // https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Vector3-sqrMagnitude.html
        //     var differenceInVectors = detectedTargetTransform.position - transform.position;
        //     var distanceFromPlayer = differenceInVectors.sqrMagnitude;
        //     
        //     if (distanceFromPlayer < attackRange)
        //     {
        //         Debug.Log("In range to attack");
        //         
        //         
        //         bInRangeToAttack = true;
        //     } 
        //     else { bInRangeToAttack = false; }
        // }
        
        // make something so that when the attack state is entered create a cooldown period here so the enemy is active and moving
        // id rather it move to jsut infront of the player or maybe just chase or patrol
        
    }

    public override void PerceptionTargetFound(Transform target)
    {
        base.PerceptionTargetFound(target);
        bInRangeToAttack = true;
    }

    public override void PerceptionTargetLost(Transform target)
    {
        base.PerceptionTargetLost(target);
        bInRangeToAttack = false;
    }

    

    
    
}
