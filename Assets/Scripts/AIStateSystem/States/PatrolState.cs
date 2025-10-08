using UnityEngine;
using Unity.VisualScripting;

public class PatrolState : IAiStates
{
    private readonly AIController aiControllerInstance;

    public PatrolState(AIController aiControllerInstance)
    {
        this.aiControllerInstance = aiControllerInstance;
    }
    public void Enter(AIController aiController)
    {
        aiControllerInstance.patrolComponentObject.enabled = true;
        // aiControllerInstance.patrolComponentObject.StopAllCoroutines();
        aiController.patrolComponentObject.BeginPatrol();

        aiController.MovementController.OnTargetReachedCaller +=
            aiController.patrolComponentObject.OnTargetReachedListener;
        Debug.Log("patrol");
    }

    public void PollPerception(AIController aiController)
    {
        if (aiController.bHasPerceivedTarget)
        {
            aiControllerInstance.setNewState(aiController.chase);
        }
    }

    public void Exit(AIController aiController)
    {
        aiControllerInstance.patrolComponentObject.enabled = false;
        // aiControllerInstance.patrolComponentObject.StopAllCoroutines();
        aiController.MovementController.OnTargetReachedCaller -=
            aiController.patrolComponentObject.OnTargetReachedListener;
    }
}
        
