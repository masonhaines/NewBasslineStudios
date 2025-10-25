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
    
    public bool bAttackFinished { get; set; } = true;

    private void FixedUpdate()
    {
        if (bAttackFinished) return; // if finished attacking leave update early

        // Debug.Log(bAttackFinished);
        timer += Time.deltaTime;
        if (timer >= firingRate)
        {
            timer -= firingRate;
            FireProjectile();
        }
    }
    
    public void Initialize(Animator animatorRef) { }
    
    
    public void StartAttack()
    {
        if (!bAttackFinished) return;
        bAttackFinished = false;
        FireProjectile();
    }
    
    void FireProjectile()
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
