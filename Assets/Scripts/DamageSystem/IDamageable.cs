using UnityEngine;

namespace Combat
{
    public interface IDamageable
    {
        void Damage(int damageAmount, GameObject damageSource, float knockBackAmount, float knockBackLiftAmount);
    }
}