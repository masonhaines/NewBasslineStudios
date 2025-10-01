using UnityEngine;
using Unity.VisualScripting;

public class PatrolState : IAiStates
{
    private AIController aiControllerInstance;
    private bool hasPercievedTarget; // this needs to be changed to get the ref from ai controller because it should be instantiated its own instance of the perception component

    public PatrolState(AIController aiControllerInstance)
    {
        this.aiControllerInstance = aiControllerInstance;
    }
    public void Enter(AIController aiController)
    {
        aiControllerInstance.patrolComponentObject.enabled = true;
        aiController.movementComponentObject.OnTargetReachedCaller +=
            aiController.patrolComponentObject.OnTargetReachedListener;
        Debug.Log("patrol");
    }

    public void PollPerception(AIController aiController)
    {
        if (hasPercievedTarget)
        {
            aiControllerInstance.setNewState(new ChaseState(this.aiControllerInstance));
        }
    }

    public void Exit(AIController aiController)
    {
        aiControllerInstance.patrolComponentObject.enabled = false;
        aiController.movementComponentObject.OnTargetReachedCaller -=
            aiController.patrolComponentObject.OnTargetReachedListener;
    }
}
        
