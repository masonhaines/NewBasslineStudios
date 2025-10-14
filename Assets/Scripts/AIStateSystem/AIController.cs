using System;

using AIStateSystem.States;
using UnityEngine;
using Random = System.Random;

// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIController : MonoBehaviour
{
    [SerializeField] private float attackRange = 3.0f;
    [SerializeField] public bool stopMovementForAttackAnimation = true;
    
    // only used to modify speed and other attributes like damage or health via co routines
    [SerializeField] private int maxAttacksBeforeReset = 0;
    [SerializeField] public float temporaryMovementSpeed = 0f;
    private float savedMoveSpeed;
    private int attackCounter;

    public PatrolState patrol;
    public ChaseState chase;
    public DeathState death;
    public AttackState attacking;
    
    public PatrolComponent patrolComponentObject;
    public ChaseComponent chaseComponentObject;
    public AttackComponent attackComponentObject;
    public HealthComponent healthComponentObject;

    // public Rigidbody2D enemyRigidBody;
    public Transform detectedTargetTransform;
    public Animator myAnimator;
    
    private IAiStates currentState;
    public ITarget MovementController;
    
    public bool bHasPerceivedTarget;
    public bool bIsAttacking;
    public bool bInRangeToAttack;
    public bool bIsDead;

    
    
    private void Awake()
    {

        detectedTargetTransform = null;
        MovementController = GetComponent<ITarget>();
        
        patrolComponentObject = GetComponent<PatrolComponent>();
        chaseComponentObject = GetComponent<ChaseComponent>();
        healthComponentObject = GetComponent<HealthComponent>();
        
        attackComponentObject = GetComponentInChildren<AttackComponent>();
        myAnimator = GetComponentInChildren<Animator>(); // this is because the animator is in the sprite child object of the enemy prefab 
        
        // enemyRigidBody = GetComponent<Rigidbody2D>();
        // add a health component listener for on death and on Hit ie taking damage
        healthComponentObject.OnDeathCaller += OnDeathListener;
        healthComponentObject.OnHitCaller += OnHitListener;
        attackComponentObject.AddToAttackCount += OnAttackCounting;
        
        attackComponentObject?.Initialize(myAnimator);
    }

    private void Start()
    {
        patrol = new PatrolState(this);
        chase = new ChaseState(this);
        death = new DeathState(this);
        attacking = new AttackState(this);
        
        chaseComponentObject.enabled = false;
        savedMoveSpeed = MovementController.GetMoveSpeed();
        setNewState(patrol);
    }

    public void PerceptionTargetFound(Transform target)
    {
        bHasPerceivedTarget = true;
        detectedTargetTransform = target;
        // Debug.Log("Target found: " + detectedTargetTransform.name);
    }

    public void PerceptionTargetLost(Transform target)
    {
        bHasPerceivedTarget = false;
        // Debug.Log("Target lost: " + detectedTargetTransform.name);
        detectedTargetTransform = null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        currentState.PollPerception();
        if ((currentState == attacking && !attackComponentObject.bAttackFinished) && stopMovementForAttackAnimation)
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
        else if (currentState == patrol && !bIsAttacking)
        {
            MovementController.OnTick();
        }


        if (bHasPerceivedTarget && detectedTargetTransform is not null)
        {
            
            // https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Vector3-sqrMagnitude.html
            var differenceInVectors = detectedTargetTransform.position - transform.position;
            var distanceFromPlayer = differenceInVectors.sqrMagnitude;
            
            if (distanceFromPlayer < attackRange)
            {
                bInRangeToAttack = true;
            } 
            else { bInRangeToAttack = false; }
        }
    }
    
    
    
    public void setNewState(IAiStates newState)
    {
        if (currentState == death)
        {
            Destroy(gameObject);
            return;
        }
        if (currentState != null) // if the current state is not valid, exit the state
        {
            currentState.Exit();
        }
        
        currentState = newState; // set the current state to the new state 
        currentState.Enter(); // call the currentstate's enter method to truly enable the state
        Debug.Log(currentState);
    }

    private void OnDeathListener()
    {
        // this really should set the enemy location to somewhere else and a system is added in the scene and checks 
        // on tick for objects with enemy tag and if they are dead.
        setNewState(death);
    }

    private void OnHitListener(Transform target)
    {
        myAnimator.SetTrigger("tOnHit");
        PerceptionTargetFound(target);
    }

    private void OnAttackCounting()
    {
        
        
        attackCounter++;
        // Debug.Log($"{name} attack count triggered");
        // Only modify speed when below threshold
        if (attackCounter < maxAttacksBeforeReset)
        {
            MovementController.SetMoveSpeed(temporaryMovementSpeed);
        }
        else
        {
            MovementController.SetMoveSpeed(savedMoveSpeed);
            attackCounter = 0;
        }
    }
}
