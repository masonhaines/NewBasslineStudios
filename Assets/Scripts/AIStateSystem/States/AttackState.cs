using Unity.VisualScripting;
using UnityEngine;

namespace AIStateSystem.States
{
    public class AttackState : IAiStates
    {
        private readonly AIController aiController;

        public AttackState(AIController aiControllerInstance)
        {
            this.aiController = aiControllerInstance;
        }
        
        public void Enter()
        {
            aiController.attackComponentObject.StartAttack();
            // Debug.Log("Attacking");
        }

        public void PollPerception()
        {
            if (!aiController.bHasPerceivedTarget) // has NOT
            {
                aiController.setNewState(aiController.patrol);
            }
            else if (!aiController.bInRangeToAttack)
            {
                aiController.setNewState(aiController.chase);
            }
            
            aiController.attackComponentObject.StartAttack();
        }

        public void Exit()
        {
            // nothing so far is needed for exit in this state
        }
    }
}