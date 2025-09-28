using System;
using UnityEngine;

// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AIController : MonoBehaviour
{
    [SerializeField] private int attackRange = 5;
    
    private PatrolState patrol;
    private ChaseState chase;
    private DeathState death;
    
    public PatrolComponent patrolComponentObject;
    public ChaseComponent chaseComponentObject;
    public AiMovementComponent movementComponentObject;
    public HealthComponent healthComponentObject;
    public Transform detectedTargetTransform;
    public Animator myAnimator;
    
    private IAiStates currentState;
    public bool bHasPerceivedTarget;
    public bool bIsAttacking;
    public bool bInRangeToAttack;
    public bool bIsDead;
    
    
    private void Awake()
    {
        patrolComponentObject = GetComponent<PatrolComponent>();
        chaseComponentObject = GetComponent<ChaseComponent>();
        movementComponentObject = GetComponent<AiMovementComponent>(); // this is for states access
        healthComponentObject = GetComponent<HealthComponent>();
        myAnimator = GetComponentInChildren<Animator>(); // this is because the animator is in the sprite child object of the enemy prefab 
        
        // add a health component listener for on death and on Hit ie taking damage
        healthComponentObject.OnDeathCaller += OnDeathListener;
        healthComponentObject.OnHitCaller += OnHitListener;

    }

    public void PerceptionTargetFound(Transform target)
    {
        bHasPerceivedTarget = true;
        detectedTargetTransform = target;
        Debug.Log("Target found: " + detectedTargetTransform.name);
    }

    public void PerceptionTargetLost(Transform target)
    {
        bHasPerceivedTarget = false;
        // var lastKnownTargetTransform = target;
        // detectedTargetTransform = lastKnownTargetTransform;
        Debug.Log("Target lost: " + detectedTargetTransform.name);
    }

    private void Start()
    {
        patrol = new PatrolState(this);
        chase = new ChaseState(this);
        death = new DeathState(this);
        setNewState(patrol);
    }

    // Update is called once per frame
    private void Update()
    {
        currentState.PollPerception(this);

        if (bHasPerceivedTarget && !healthComponentObject.GetIsKnockedBack())
        {
            if (currentState != chase)
            {
                setNewState(chase);
            }
            chaseComponentObject.GetNewWaypoint(detectedTargetTransform);
        }

        if (bHasPerceivedTarget && detectedTargetTransform is not null)
        {
            
            // https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Vector3-sqrMagnitude.html
            var differenceInVectors = detectedTargetTransform.position - transform.position;
            var distanceFromPlayer = differenceInVectors.sqrMagnitude;
            if (distanceFromPlayer < attackRange * attackRange)
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
            currentState.Exit(this);
        }
        
        currentState = newState; // set the current state to the new state 
        currentState.Enter(this); // call the currentstate's enter method to truly enable the state

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
}
