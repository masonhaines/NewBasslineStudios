using System;
using UnityEngine;



public class AiMovementComponent : MonoBehaviour
{
    public event System.Action OnTargetReachedCaller = delegate { };
    public enum MovementType
    {
        YOnly,
        XOnly,
        XAndY
    };
    
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private bool groundOnly = true;
    [SerializeField] private MovementType movementType;

    private AIController aiController;
    private Rigidbody2D moversRigidbody2D;
    private Vector2 targetLocation;
    public LayerMask groundLayer;
    public Collider2D groundCollider;
    
    private Vector2 lastKnownPosition;
    public bool bHasReachedTarget;
    public float facingDirection { get; private set; }




    private void Awake()
    {
        aiController = GetComponent<AIController>();
        groundCollider = GetComponent<Collider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        moversRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Moving()
    {
        Debug.DrawLine(targetLocation, moversRigidbody2D.position, Color.red);
        // moversRigidbody2D.transform.position = Vector2.MoveTowards(moversRigidbody2D.transform.position, targetLocation, moveSpeed * Time.deltaTime);
        Vector2 moveTowardsPosition = Vector2.MoveTowards(moversRigidbody2D.transform.position, targetLocation, moveSpeed * Time.fixedDeltaTime);
        
        switch (movementType)
        {
            case MovementType.XOnly:
                bHasReachedTarget = Mathf.Abs(moversRigidbody2D.position.x - targetLocation.x) <= 0.5f;
                moveTowardsPosition = new Vector2(moveTowardsPosition.x, moversRigidbody2D.position.y);
                break;
            case MovementType.XAndY:
                bHasReachedTarget = Vector2.Distance(moversRigidbody2D.position, targetLocation) <= 0.5f;
                
                break;
            case MovementType.YOnly:
                bHasReachedTarget = Mathf.Abs(moversRigidbody2D.position.y - targetLocation.y) <= 0.5f;
                moveTowardsPosition = new Vector2(moversRigidbody2D.position.x, targetLocation.y);
                break;
            default:
                bHasReachedTarget = false;
                break;
        }
        
        
        if (bHasReachedTarget)
        {
            // moveSpeed = startMoveSpeed;
            OnTargetReachedCaller?.Invoke();
            return;
        }
        
        // ***** Chat helped with flip lock and delta time toggling
        Vector2 direction = targetLocation - moversRigidbody2D.position;
        
        // Debug.Log(direction.x + "-----------------------------------------Direction" );
        switch (direction.x)
        {
            case > 0.1f:
                // SpriteRenderer.flipX = true; // only flips sprite
                transform.localScale = new Vector3(1, 1, 1);  // face left
                facingDirection = 1;
                break;
            case < -0.1f:
                // SpriteRenderer.flipX = false; // only flips sprite
                transform.localScale = new Vector3(-1, 1, 1);  // face left
                facingDirection = -1;
                break;
        }
        
        // where movement is actually happening 
        moversRigidbody2D.MovePosition(moveTowardsPosition);
    }
    
    // this is called from inside the AI controller
    public void OnTick()
    {
        
        if (aiController.healthComponentObject.GetIsKnockedBack())
        {
            return;
        }
        
        if (!bHasReachedTarget && (!groundOnly || groundCollider.IsTouchingLayers(groundLayer)))
        {
            Moving();
        }
    }
    
    public void NewTargetLocation(Vector2 moveToTargetLocation)
    {
        // Debug.Log(targetLocation + "target location changed in new target location in move component");
        targetLocation = moveToTargetLocation;
        bHasReachedTarget = false;
        // Debug.DrawLine(aiController.transform.position, moversRigidbody2D.position, Color.red);

    }
    
    public Vector2 GetMovementDirection(Vector2 currentPosition)
    {
        // get current direction moving from velocity 
        Vector2 movementDirection = new Vector2(
            moversRigidbody2D.linearVelocity.x,
            moversRigidbody2D.linearVelocity.y
        ).normalized;
        return movementDirection;
    }

    public void StopMovement()
    {
        moversRigidbody2D.linearVelocity = Vector2.zero;
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        // startMoveSpeed = moveSpeed;
        moveSpeed = newMoveSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public Vector2 GetTargetLocation()
    {
        return targetLocation;
    }
    
    private void OnDisable()
    {
        CancelInvoke();
    }
    
}
