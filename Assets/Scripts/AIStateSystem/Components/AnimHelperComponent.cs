using System.Collections;
using UnityEngine;

public class AnimHelperComponent : MonoBehaviour
{
    [SerializeField] private Color invulnerableColor;
    protected HealthComponent healthComponent;
    protected SpriteRenderer sprite;
    protected Animator animator;
    
    private Color OriginalColor;
    
    void Start()
    {
            healthComponent = GetComponentInParent<HealthComponent>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            OriginalColor = sprite.color;
    }

    public void SetIsInvulnerable()
    {
        healthComponent.isInvulnerable = true;
    }

    public void UnsetIsInvulnerable()
    {
        healthComponent.isInvulnerable = false;
    }

    private void updateColor()
    {
        if (healthComponent.isInvulnerable == true)
        {
            sprite.color = invulnerableColor;
        }
        else if (healthComponent.isInvulnerable == false)
        {
            sprite.color = OriginalColor;
        }
    }

    public void RemoveHurtAnimation()
    {
        if (healthComponent.hitCount % 3 == 0 )
        {
            animator.SetBool("bIsTank", true);
            StartCoroutine(TankTimer());
        }
    }

    private IEnumerator TankTimer()
    {
        yield return new WaitForSeconds(10.0f);
        animator.SetBool("bIsTank", false);
    }
}
