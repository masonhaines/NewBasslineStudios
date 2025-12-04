using System.Collections;
using UnityEngine;

public class EnemySimpleJumpComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemyRigidBody;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForceUp = 5f;
    [SerializeField] private float jumpForceForward = 2f;

    private AIControllerMinotaur aiController;

    private void Awake()
    {
        if (!enemyRigidBody)
        {
            enemyRigidBody = GetComponentInParent<Rigidbody2D>();
        }

        aiController = GetComponentInParent<AIControllerMinotaur>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) == 0)
        {
            return;
        }
        
        aiController.jumping = true;
        
        if (enemyRigidBody == null || aiController == null) return;
        if (!aiController.bHasPerceivedTarget) return;
        Debug.Log("Made it to the jump");
        Vector2 jumpDirection =
            new Vector2(aiController.facingDirection * jumpForceForward, jumpForceUp);
        
        enemyRigidBody.linearVelocity = Vector2.zero;
        enemyRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);
        StartCoroutine(JumpTimer());

    }
    
    protected IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(.2f);
        aiController.jumping = false;
    }
}