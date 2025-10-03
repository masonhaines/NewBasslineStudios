using UnityEngine;
using Unity.VisualScripting;

public class PatrolState : IAiStates
{
    private AIController aiControllerInstance;

    public PatrolState(AIController aiControllerInstance)
    {
        this.aiControllerInstance = aiControllerInstance;
    }
    public void Enter(AIController aiController)
    {
        aiControllerInstance.patrolComponentObject.enabled = true;
        aiController.patrolComponentObject.BeginPatrol();

        aiController.MovementController.OnTargetReachedCaller +=
            aiController.patrolComponentObject.OnTargetReachedListener;
        Debug.Log("patrol");
    }

    public void PollPerception(AIController aiController)
    {
        if (aiController.bHasPerceivedTarget)
        {
            aiControllerInstance.setNewState(new ChaseState(this.aiControllerInstance));
        }
    }

    public void Exit(AIController aiController)
    {
        aiControllerInstance.patrolComponentObject.enabled = false;
        aiController.MovementController.OnTargetReachedCaller -=
            aiController.patrolComponentObject.OnTargetReachedListener;
    }
}
        
