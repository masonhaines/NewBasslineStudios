using UnityEngine;


public interface IAiStates
{
    void Enter(); // enable the current state
    void PollPerception(); 
    void Exit(); // disbale current state 
}

