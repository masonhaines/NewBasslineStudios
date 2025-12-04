using UnityEngine;

public class EnemySimpleJumpComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemyRigidBody;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForceUp = 5f;
    [SerializeField] private float jumpForceForward = 2f;
    [SerializeField] private float minTimeBetweenJumps = 0.5f;

    private AIController aiController;
    private float timeSinceLastJump;

    private void Awake()
    {
        if (!enemyRigidBody)
        {
            enemyRigidBody = GetComponentInParent<Rigidbody2D>();
        }

        aiController = GetComponentInParent<AIController>();
    }

    private void Update()
    {
        timeSinceLastJump += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // only respond to layers in groundLayer mask
        if (((1 << other.gameObject.layer) & groundLayer) == 0)
        {
            return;
        }

        if (enemyRigidBody == null || aiController == null)
        {
            return;
        }

        if (!aiController.bHasPerceivedTarget)
        {
            // only jump when actually chasing the player
            return;
        }

        if (timeSinceLastJump < minTimeBetweenJumps)
        {
            return;
        }

        // if you want only vertical jump, set jumpForceForward to 0 in the inspector
        Vector2 jumpDirection =
            new Vector2(aiController.facingDirection * jumpForceForward, jumpForceUp);

        enemyRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);
        timeSinceLastJump = 0f;
    }
}