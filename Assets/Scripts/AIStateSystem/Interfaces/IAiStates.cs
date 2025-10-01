using UnityEngine;


public interface IAiStates
{
    void Enter(AIController aiController); // enable the current state
    void PollPerception(AIController aiController); 
    void Exit(AIController aiController); // disbale current state 
}

