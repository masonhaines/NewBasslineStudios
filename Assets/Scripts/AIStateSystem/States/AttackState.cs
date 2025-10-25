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
        
        aiController.AttackController.StartAttack();
        
        // Debug.Log("Attacking");
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
        
        if (aiController.AttackController.bAttackFinished)
        {
            aiController.AttackController.StartAttack();
        }
    }

    public void Exit()
    {
        aiController.AttackController.bAttackFinished = true;
    }
}
