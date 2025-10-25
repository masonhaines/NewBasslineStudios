using UnityEngine;

public interface ICoreAttack
{
    // The main way to initiate an attack.
    void StartAttack(); 
    // Initialization with a common reference (adjust parameter if Animator is not universal).
    
    void Initialize(Animator animatorRef);
    
    // Read-only property for client to check if a new attack can be started.
    bool bAttackFinished { get; set; }
    
  
}