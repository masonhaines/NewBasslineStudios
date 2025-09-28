using UnityEngine;
using Unity.VisualScripting;

public class DeathState : IAiStates
{
    private AIController aiControllerInstance;

    public DeathState(AIController aiControllerInstance)
    {
        this.aiControllerInstance = aiControllerInstance;
    }
    
    // this state is being entered directly from the ai controller 
    public void Enter(AIController aiController)
    {
        aiControllerInstance.myAnimator.SetBool("bIsDead", true);
        aiControllerInstance.bIsDead = true;
    }

    public void PollPerception(AIController aiController)
    {
        // throw new System.NotImplementedException();
    }

    public void Exit(AIController aiController)
    {
        // throw new System.NotImplementedException();
    }
}
        
