using UnityEngine;


public class AttackComponent: MonoBehaviour
{
    private Animator animator;
    private DamageComponent damageComponent;
    
    public void Initialize(Animator animatorRef, DamageComponent damageRef)
    {
        animator = animatorRef;
        damageComponent = damageRef;
    }

    public void StartAttack()
    {
        
        animator.SetTrigger("tHasReachedTarget");
    }

    public void DamageEnabled()
    {
        damageComponent.DamageEnabled();
    }

    public void DamageDisabled()
    {
        damageComponent.DamageDisabled();
    }

}
