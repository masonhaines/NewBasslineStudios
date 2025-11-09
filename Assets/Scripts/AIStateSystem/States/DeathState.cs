using UnityEngine;
using Unity.VisualScripting;

public class DeathState : IAiStates
{
    private readonly AIController aiController;

    public DeathState(AIController aiControllerInstance)
    {
        this.aiController = aiControllerInstance;
    }
    
    // this state is being entered directly from the ai controller 
    public void Enter()
    {
        // Debug.Log("Enter death state");
        aiController.myAnimator.SetBool("bIsDead", true);
        // aiController.bIsDead = true;
    }

    public void PollPerception()
    {
        // throw new System.NotImplementedException();
    }

    public void Exit()
    {
        // throw new System.NotImplementedException();
    }
}
        
