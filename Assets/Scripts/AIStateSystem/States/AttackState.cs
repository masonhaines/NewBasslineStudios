using Unity.VisualScripting;
using UnityEngine;

namespace AIStateSystem.States
{
    public class AttackState : IAiStates
    {
        private readonly AIController aiControllerInstance;

        public AttackState(AIController aiControllerInstance)
        {
            this.aiControllerInstance = aiControllerInstance;
        }
        
        public void Enter(AIController aiController)
        {
            aiController.attackComponentObject.StartAttack();
            Debug.Log("Attacking");
        }

        public void PollPerception(AIController aiController)
        {
            if (!aiControllerInstance.bHasPerceivedTarget) // has NOT
            {
                aiControllerInstance.setNewState(aiController.patrol);
            }
            else if (!aiControllerInstance.bInRangeToAttack)
            {
                aiControllerInstance.setNewState(aiController.chase);
            }
        }

        public void Exit(AIController aiController)
        {
            // nothing so far is needed for exit in this state
        }
    }
}