using UnityEngine;
using Unity.VisualScripting;

public class PatrolState : IAiStates
{
    private readonly AIController aiController;

    public PatrolState(AIController aiControllerInstance)
    {
        this.aiController = aiControllerInstance;
    }
    public void Enter()
    {
        aiController.patrolComponentObject.enabled = true;
        // aiControllerInstance.patrolComponentObject.StopAllCoroutines();
        aiController.patrolComponentObject.BeginPatrol();

        aiController.MovementController.OnTargetReachedCaller +=
            aiController.patrolComponentObject.OnTargetReachedListener;
        // Debug.Log("patrol");
    }

    public void PollPerception()
    {
        if (aiController.bHasPerceivedTarget)
        {
            aiController.setNewState(aiController.chase);
        }
    }

    public void Exit()
    {
        aiController.patrolComponentObject.enabled = false;
        // aiControllerInstance.patrolComponentObject.StopAllCoroutines();
        aiController.MovementController.OnTargetReachedCaller -=
            aiController.patrolComponentObject.OnTargetReachedListener;
    }
}
        
