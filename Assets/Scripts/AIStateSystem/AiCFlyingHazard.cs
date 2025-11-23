using System;
using System.Collections;

using UnityEngine;


public class AiCFlyingHazard : AIController
{
    public FreezeUp hazardTrait;
    private bool bIsDead = false;

    protected override void Awake()
    {
        base.Awake(); 

        hazardTrait = GetComponent<FreezeUp>();

        
        if (chaseComponentObject != null)
            chaseComponentObject.enabled = false;

        if (attackComponentObject != null)
            attackComponentObject.enabled = false;

        stopMovementForAttackAnimation = false;
    }

    protected override void Update()
    {
        
        // base.Update();

        if (bIsDead)
        {
            enemyRigidBody.linearVelocity = Vector2.zero;
        }
    }
    
    protected override void OnHitListener(Transform target, int damage)
    {
        
        base.OnHitListener(target, damage);

        
        if (hazardTrait != null)
        {
            hazardTrait.enabled = false;
        }
    }

    protected override void OnDeathListener()
    {
        bIsDead = true;
        base.OnDeathListener();
    }
}