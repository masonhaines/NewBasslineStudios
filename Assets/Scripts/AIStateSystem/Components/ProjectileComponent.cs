using System;
using System.Collections;
using UnityEngine;


public class ProjectileComponent: MonoBehaviour, ICoreAttack
{
    public GameObject projectilePrefab;
    public Transform projectilePosition;

    [SerializeField] private float incrementTime;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > incrementTime)
        {
            timer = 0;
            FireProjectile();
        }
    }
    public bool bAttackFinished { get; set; } = true;
    
    
    public void StartAttack()
    {
        
    }

    

    public void Initialize(Animator animatorRef)
    {
        
    }

    void FireProjectile()
    {
        Instantiate(projectilePrefab, projectilePosition.position, Quaternion.identity);
    }
}
