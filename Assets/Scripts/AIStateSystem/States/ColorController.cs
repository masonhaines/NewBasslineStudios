using System.Collections;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    
    [SerializeField] private Color overrideColor = Color.cyan;
    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    [SerializeField] private float recolorDuration = 0.5f;
    [SerializeField] public HealthComponent healthComponent;
    [SerializeField] public bool bInvulnerableOnRecolor = false;
    private Color originalColor;
    private bool hasOriginalColor;
    
    

    protected void Awake()
    {
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null)
            {
                targetSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        if (targetSpriteRenderer != null)
        {
            originalColor = targetSpriteRenderer.color;
            hasOriginalColor = true;
        }
    }

    // Call to override colors
    public void ColorSprite()
    {
        if (targetSpriteRenderer == null) return;
        targetSpriteRenderer.color = overrideColor;
        if (bInvulnerableOnRecolor)
        {
            healthComponent.isInvulnerable = true;
        }
    }

    // Call to revert to the color captured on awake
    public void RevertSpriteColor()
    {
        if (targetSpriteRenderer == null || !hasOriginalColor) return;
        targetSpriteRenderer.color = originalColor;
        if (bInvulnerableOnRecolor)
        {
            healthComponent.isInvulnerable = false;
        }
    }

    public void AnimDrivenRecolor()
    {
        ColorSprite();
        StartCoroutine(RevertSpriteColorOnRoutine());
    }

    public IEnumerator RevertSpriteColorOnRoutine()
    {
        yield return new WaitForSeconds(recolorDuration);
        RevertSpriteColor();
    }
}