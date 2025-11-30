


using UnityEngine;

public class AIControllerMeleeBanshee: AIController
{
    [SerializeField] private AiPerceptionComponent perception;
    protected override void FixedUpdate()
    {
        
        if (healthComponentObject.isInvulnerable)
        {
            // MovementController.moveSpeed *= .5f;
            MovementController.SetMoveSpeed(2);
            perception.enabled = false;
        }
        else if (!healthComponentObject.isInvulnerable)
        {
            // MovementController.moveSpeed = MovementController.initMoveSpeed;
            MovementController.SetMoveSpeed(MovementController.initMoveSpeed);
            perception.enabled = true;
            myAnimator.SetBool("bTimedOut", false);
        }
        
        base.FixedUpdate();
    }

    protected override void Update()
    {
        if (healthComponentObject.isInvulnerable)
        {
            // setNewState(patrol);
            myAnimator.SetBool("bTimedOut", true);
            // return;
        }
        base.Update();
    }
}

    
