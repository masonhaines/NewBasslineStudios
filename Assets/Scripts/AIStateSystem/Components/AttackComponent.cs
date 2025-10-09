using UnityEngine;


public class AttackComponent: MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Collider2D hitbox;
    
    public void Initialize(Animator animatorRef)
    {
        animator = animatorRef;
        DamageDisabled();
    }

    public void StartAttack()
    {
        
        animator.SetTrigger("tCanAttackTarget");
    }

    public void DamageEnabled()
    {
        hitbox.enabled = true;
    }

    public void DamageDisabled()
    {
        hitbox.enabled = false;
    }
    
}
