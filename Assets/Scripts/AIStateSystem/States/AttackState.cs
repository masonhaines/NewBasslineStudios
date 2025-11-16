using Unity.VisualScripting;
using UnityEngine;


public class AttackState : IAiStates
{
    private readonly AIController aiController;
    

    public AttackState(AIController aiControllerInstance)
    {
        this.aiController = aiControllerInstance;
    }
    
    public void Enter()
    {
        
        if (!aiController.AttackController.bAttacking &&
            !aiController.AttackController.bPrimaryAttackActive)
        {
            aiController.AttackController.StartAttack();
            // Debug.Log("Attacking");
        }
        
    }

    public void PollPerception()
    {
        if (!aiController.bHasPerceivedTarget) // has NOT
        {
            aiController.setNewState(aiController.patrol);
            return;
        }
        else if (!aiController.bInRangeToAttack)
        {
            aiController.setNewState(aiController.chase);
            return;
        }
        
        if (!aiController.AttackController.bAttacking &&
            !aiController.AttackController.bPrimaryAttackActive)
        {
            Debug.Log($"[AttackState] Ready to StartAttack at {Time.time}, " +
                      $"attacking={aiController.AttackController.bAttacking}, " +
                      $"primaryActive={aiController.AttackController.bPrimaryAttackActive}");
            aiController.AttackController.StartAttack();
        }
    }

    public void Exit()
    {
        // aiController.AttackController.bAttackFinished = true;
    }
}
