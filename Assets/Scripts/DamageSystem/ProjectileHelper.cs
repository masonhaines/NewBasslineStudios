using UnityEngine;

public class ProjectileHelper : MonoBehaviour
{
    [SerializeField] ProjectileComponent projectileOne;
    [SerializeField] ProjectileComponent projectileTwo;

    void Awake()
    {
        ProjectileComponent[] projectiles = GetComponentsInChildren<ProjectileComponent>();
        if (projectiles.Length > 0) projectileOne = projectiles[0];
        if (projectiles.Length > 1) projectileTwo = projectiles[1];
    }

    public void FireProjectile()
    {
        projectileOne.FireProjectile();
        projectileTwo.FireProjectile();
    }
}
