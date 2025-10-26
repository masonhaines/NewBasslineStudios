using UnityEngine;

public class PlayerAnimationRelay : MonoBehaviour
{
    private PlayerController2D player;

    void Awake()
    {
        player = GetComponentInParent<PlayerController2D>();
    }

    // For animation events
    public void StartAttack()
    {
        player.StartAttack();
    }

    public void EndAttack()
    {
        player.EndAttack();
    }
}
