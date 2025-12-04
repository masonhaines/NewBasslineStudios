using System;
using System.Collections;

using UnityEngine;


// SET UP
// collider that is keeping the enemy from falling through the map is going to need to have the noFriction material
public class AiControllerFlying : AIController
{
    public ProjectileComponent projectileComponentObject;
    // private bool bIsDead = false;
    [SerializeField] public bool bNeedsInit = true;
    protected override void Awake()
    {
        base.Awake();
        
        projectileComponentObject = GetComponent<ProjectileComponent>();
        AttackController = GetComponent<ICoreAttack>();
        AttackController = projectileComponentObject;
        myAnimator = GetComponent<Animator>(); // this is because the animator is in the sprite child object of the enemy prefab 
        if (bNeedsInit)
        {
            AttackController?.Initialize(myAnimator);
        }
    }

    protected override void Start()
    {
        base.Start();
        projectileComponentObject.enabled = false;
    }

    protected override void Update()
    {
        //     if (currentState == death || bIsDead)
        //     {
        //         // enemyRigidBody.linearVelocity = Vector2.zero;
        //         //
        //         // setNewState(patrol);
        //         return;
        //     }
        //     currentState.PollPerception();
        //     if (currentState == attacking)
        //     {
        //         projectileComponentObject.enabled = true;
        //     }
        //     else
        //     {
        //         projectileComponentObject.enabled = false;
        //     }
        //     
        //     // make something so that when the attack state is entered create a cooldown period here so the enemy is active and moving
        //     // id rather it move to jsut infront of the player or maybe just chase or patrol
        //     

        base.Update();
        if (!bInRangeToAttack)
        {
            // projectileComponentObject.enabled = false;  
        }
}

    public override void PerceptionTargetFound(Transform target)
    {
        base.PerceptionTargetFound(target);
        projectileComponentObject.enabled = true;
    }

    public override void PerceptionTargetLost(Transform target)
    {
        base.PerceptionTargetLost(target);
        projectileComponentObject.enabled = false;
        if (currentState == death) return;
        StartCoroutine(ReturnToPatrol());
    }

    public IEnumerator ReturnToPatrol()
    {
        yield return new WaitForSeconds(1.5f);
        setNewState(patrol);
    }

    // protected override void OnHitListener(Transform target, int damage)
    // {
    //     projectileComponentObject.enabled = false;
    //     if (!RecolorOnHit) return;
    //     if (sprite)
    //     {
    //         setColor();
    //     }
    //     
    //     // StartCoroutine(ResetColor());
    // }

    protected override void OnDeathListener()
    {
        base.OnDeathListener();
        StopAllCoroutines();
        projectileComponentObject.enabled = false;
        // bIsDead = true;
    }
    
    
}
