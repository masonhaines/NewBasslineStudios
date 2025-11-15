using System;
using System.Collections;

using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIControllerSkeletonChild : AIController
{
    // [SerializeField] private float attackRange = 3.0f;
    // [SerializeField] public bool stopMovementForAttackAnimation = true;
    //
    // // only used to modify speed and other attributes like damage or health via co routines
    // [SerializeField] private int maxAttacksBeforeReset = 0;
    // [SerializeField] public float temporaryMovementSpeed = 0f;
    // [SerializeField] public bool RecolorOnHit = true;
    //
    // private float savedMoveSpeed;
    // private int attackCounter;
    //
    //
    // public PatrolState patrol;
    // public ChaseState chase;
    // public DeathState death;
    // public AttackState attacking;
    //
    // public PatrolComponent patrolComponentObject;
    // public ChaseComponent chaseComponentObject;
    // public AttackComponent attackComponentObject;
    // public HealthComponent healthComponentObject;
    // public AiMovementComponent MovementController;
    //
    //
    //
    // public Rigidbody2D enemyRigidBody;
    // public Transform detectedTargetTransform;
    // public Animator myAnimator;
    //
    //
    // protected IAiStates currentState;
    // public ICoreAttack AttackController;
    //
    // public bool bHasPerceivedTarget;
    // public bool bIsAttacking;
    // public bool bInRangeToAttack;
    // // public bool bIsDead;
    //
    //
    //
    // protected virtual void Awake()
    // {
    //
    //     detectedTargetTransform = null;
    //     MovementController = GetComponent<AiMovementComponent>();
    //     patrolComponentObject = GetComponent<PatrolComponent>();
    //     chaseComponentObject = GetComponent<ChaseComponent>();
    //     healthComponentObject = GetComponent<HealthComponent>();
    //     
    //     attackComponentObject = GetComponentInChildren<AttackComponent>();
    //     AttackController = GetComponentInChildren<ICoreAttack>();
    //     AttackController = attackComponentObject;
    //     myAnimator = GetComponentInChildren<Animator>(); // this is because the animator is in the sprite child object of the enemy prefab 
    //     
    //     enemyRigidBody = GetComponent<Rigidbody2D>();
    //     healthComponentObject.OnDeathCaller += OnDeathListener;
    //     healthComponentObject.OnHitCaller += OnHitListener;
    //     attackComponentObject.AddToAttackCount += OnAttackCounting;
    //
    //
    //     AttackController?.Initialize(myAnimator); // if the ai controller has a ref to, call initialize 
    //     
    //     sprite = GetComponentInChildren<SpriteRenderer>();
    //     originalColor = sprite.color;
    // }
    //
    // private SpriteRenderer sprite;
    // private Color originalColor;
    // protected void Start()
    // {
    //     patrol = new PatrolState(this);
    //     chase = new ChaseState(this);
    //     death = new DeathState(this);
    //     attacking = new AttackState(this);
    //     
    //     
    //     chaseComponentObject.enabled = false;
    //     savedMoveSpeed = MovementController.GetMoveSpeed();
    //     setNewState(patrol);
    //     
    //
    // }
    //
    // public virtual void PerceptionTargetFound(Transform target)
    // {
    //     bHasPerceivedTarget = true;
    //     detectedTargetTransform = target;
    //     // Debug.Log("Target found: " + detectedTargetTransform.name);
    // }
    //
    // public virtual void PerceptionTargetLost(Transform target)
    // {
    //     bHasPerceivedTarget = false;
    //     // Debug.Log("Target lost: " + detectedTargetTransform.name);
    //     detectedTargetTransform = null;
    // }
    //
    // // Update is called once per frame
    // protected virtual void Update()
    // {
    //     if (currentState == death) return;
    //     currentState.PollPerception();
    //
    //     if (bHasPerceivedTarget && detectedTargetTransform is not null)
    //     {
    //         
    //         // https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Vector3-sqrMagnitude.html
    //         var differenceInVectors = detectedTargetTransform.position - transform.position;
    //         var distanceFromPlayer = differenceInVectors.sqrMagnitude;
    //         
    //         if (distanceFromPlayer < attackRange)
    //         {
    //             // Debug.Log("In range to attack");
    //             
    //             
    //             bInRangeToAttack = true;
    //         } 
    //         else { bInRangeToAttack = false; }
    //     }
    // }
    //
    protected override void FixedUpdate()
    {
        
        if (currentState == death)
        {
            
            return;
        }
        if ((currentState == attacking && !AttackController.bAttackFinished) && (stopMovementForAttackAnimation && attackComponentObject.bAttackTwoFinished))
        {
            
            MovementController.StopMovement();
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
            // myAnimator.SetBool("bMoving", true);
            MovementController.OnTick();
        }
    
    }
    //
    // public override void setNewState(IAiStates newState)
    // {
    //     if (currentState == newState) return;
    //     if (currentState == death)
    //     {
    //         // Destroy(gameObject);
    //         return;
    //     }
    //     if (currentState != null) // if the current state is not valid, exit the state
    //     {
    //         currentState.Exit();
    //     }
    //     
    //     currentState = newState; // set the current state to the new state 
    //     currentState.Enter(); // call the currentstate's enter method to truly enable the state
    //     // Debug.Log(currentState);
    // }
    //
    // protected void OnDeathListener()
    // {
    //     // this really should set the enemy location to somewhere else and a system is added in the scene and checks 
    //     // on tick for objects with enemy tag and if they are dead.
    //     setNewState(death);
    //     // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("Player"), true);
    //     // GetComponent<Collider2D>().enabled = f;        
    //     gameObject.layer = LayerMask.NameToLayer("dead");
    //     myAnimator.SetBool("bIsDead", true);
    // }
    //
    
    
    // called everytime the attack trigger is pulled
    protected override void OnAttackCounting()
    {
        localAttackCounter++;
        // Debug.Log($"{name} attack count triggered");
        if (localAttackCounter > maxAttacksBeforeReset)
        {
            stopMovementForAttackAnimation = false;
            attackComponentObject.StartAttackTwo();
            localAttackCounter = 0;
        }

        stopMovementForAttackAnimation = true;
    }

    // private IEnumerator ResetColor()
    // {
    //     yield return new WaitForSeconds(0.6f);
    //     sprite.color = originalColor;
    // }
    
}
