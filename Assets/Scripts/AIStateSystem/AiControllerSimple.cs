using System;
using System.Collections;
using UnityEngine;


public class AiControllerSimple : MonoBehaviour
{
    public ProjectileComponent projectileComponentObject;
    private bool bInRangeToAttack;

    [Header("Sprite Pulse")]
    [SerializeField] private Transform spriteRootTransform; // for child sprite obj 

    [SerializeField] private float minimumScale = 0.8f;
    [SerializeField] private float maximumScale = 1.5f;
    [SerializeField] private float pulseSpeed = 2f; 
    private bool executeThisFrame;

    protected void Awake()
    {
        projectileComponentObject = GetComponent<ProjectileComponent>();
        // AttackController = GetComponent<ICoreAttack>();

    }

    void Start()
    {
        projectileComponentObject.enabled = false;
        projectileComponentObject.bPrimaryAttackActive = false;
    }

    void Update()
    {
        if (spriteRootTransform == null)
        {
            return; 
        }

        
        float percentOfMax = spriteRootTransform.localScale.x / maximumScale;
        if (bInRangeToAttack && percentOfMax >= 0.9f)
        {
            projectileComponentObject.enabled = true;
            projectileComponentObject.bPrimaryAttackActive = false;

            projectileComponentObject.StartAttack();
        }
        else
        {
            projectileComponentObject.enabled = false;
        }

        if (executeThisFrame)
        {
            float pulseValue = Mathf.PingPong(Time.time * pulseSpeed, 1f);

            float currentScale = Mathf.Lerp(minimumScale, maximumScale, pulseValue);
            
            spriteRootTransform.localScale = new Vector3(currentScale, currentScale, 1f);
        }

        executeThisFrame = !executeThisFrame;
    }

    public void PerceptionTargetFound(Transform target)
    {
        bInRangeToAttack = true;
    }

    public void PerceptionTargetLost(Transform target)
    {
        bInRangeToAttack = false;
    }
}
