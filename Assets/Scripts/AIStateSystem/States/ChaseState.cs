using UnityEngine;
using Unity.VisualScripting;

public class ChaseState : IAiStates
{
    private readonly AIController aiController;

    public ChaseState(AIController aiControllerInstance)
    {
        this.aiController = aiControllerInstance;
    }
    public void Enter()
    {
        aiController.chaseComponentObject.enabled = true;
        aiController.chaseComponentObject.BeginChase(aiController.detectedTargetTransform);
        
        // aiController.MovementController.OnTargetReachedCaller +=
        //     aiController.chaseComponentObject.OnTargetReachedListener;
        // Debug.Log("Chase");

    }

    public void PollPerception()
    {
        if (!aiController.bHasPerceivedTarget) // has NOT
        {
            aiController.setNewState(aiController.patrol);
        }
        else if (aiController.bInRangeToAttack && aiController.bHasPerceivedTarget)
        {
            aiController.setNewState(aiController.attacking);
        }
    }

    public void Exit()
    {
        aiController.chaseComponentObject.enabled = false;
        // aiController.MovementController.OnTargetReachedCaller -=
        //     aiController.chaseComponentObject.OnTargetReachedListener;
    }
}
        
