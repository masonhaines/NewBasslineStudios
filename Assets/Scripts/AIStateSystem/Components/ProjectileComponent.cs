using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileComponent: MonoBehaviour, ICoreAttack
{
    public GameObject projectilePrefab;
    public Transform projectilePosition;

    [SerializeField] private float firingRate;
    [SerializeField] private float destructionRate;
    private float timer;
    // private bool bIsAttacking; 
    public readonly Queue<EnemyProjectile> ActiveProjectiles = new();
    
    public bool bPrimaryAttackActive { get; set; } = true;
    public bool bAttacking { get; set; }
    private bool bInitialized = false;
    public bool bNeedsInvokation = false;
    private Animator animator;



    private void FixedUpdate()
    {
        if (!bPrimaryAttackActive || bNeedsInvokation) return; // if finished attacking leave update early
        if (bAttacking) return;
        // Debug.Log("firing");
        timer += Time.deltaTime;
        if (timer >= firingRate)
        {
            timer -= firingRate;
            if (bInitialized)
            {
                animator.SetTrigger("tFireProjectile");
            }
            else
            {
                FireProjectile();
            }
        }
    }

    public void Initialize(Animator animatorRef)
    {
        if(animatorRef == null) return;
        animator = animatorRef;
        bInitialized = true;
    }

    public void InitUpdate()
    {
        if (bPrimaryAttackActive) return;
        bPrimaryAttackActive = true;
    }
    
    
    public void StartAttack()
    {
        if (bPrimaryAttackActive) return;
        bPrimaryAttackActive = true;
        bAttacking = true;
        FireProjectile();
    }
    
    public void FireProjectile()
    {
        if (!projectilePrefab || !projectilePosition) return;
        
        var pGameObject = Instantiate(projectilePrefab, projectilePosition.position, Quaternion.identity);
        var projectileRef = pGameObject.GetComponent<EnemyProjectile>();
        if (projectileRef)
        {
            ActiveProjectiles.Enqueue(projectileRef);
            StartCoroutine(DestroyProjectilesInQueue());
        }
        else
        {
            pGameObject.SetActive(false);
            Debug.Log("projectile ref is null");
        }
        // Debug.Log("runnign fire!");
    }

    private IEnumerator DestroyProjectilesInQueue()
    {
        yield return new WaitForSeconds(destructionRate);
        if (ActiveProjectiles.Count > 0)
        {
            var projectile = ActiveProjectiles.Dequeue();
            if (projectile)
            {
                Destroy(projectile.gameObject); 
            }
        }
    }
}
