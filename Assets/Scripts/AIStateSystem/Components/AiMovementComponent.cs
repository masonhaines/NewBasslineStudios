using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;
public class AiMovementComponent : MonoBehaviour, ITarget
{
    public event System.Action OnTargetReachedCaller = delegate { };
    public bool bHasReachedTarget { get; set; }

    public enum MovementType
    {
        YOnly,
        XOnly,
        XAndY
    };
    
    [SerializeField] private float moveSpeed = 5f;
    // [SerializeField] private bool bLocalHasMovedToTarget = false;
    [SerializeField] private bool groundOnly = true;
    [SerializeField] private MovementType movementType;
    
    private AIController aiController;
    private Rigidbody2D moversRigidbody2D;
    private Vector2 targetLocation;
    public LayerMask groundLayer;
    public PolygonCollider2D groundCollider;
    public SpriteRenderer spriteRenderer;
    
    private Vector2 lastKnownPosition;
    private float timeCheckForBlocked;
    
    private void Awake()
    {
        aiController = GetComponent<AIController>();
        groundCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        moversRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void OnTick()
    {
        if (aiController.healthComponentObject.GetIsKnockedBack())
        {
            return;
        }
        if (!bHasReachedTarget)
        {
            if (groundOnly || groundCollider.IsTouchingLayers(groundLayer))
            {
                Moving();
            }
        }

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
                break;
            case < -0.1f:
                // SpriteRenderer.flipX = false; // only flips sprite
                transform.localScale = new Vector3(-1, 1, 1);  // face left
                break;
        }
        
        // where movement is actually happening 
        moversRigidbody2D.MovePosition(moveTowardsPosition);
    }

    // this whole thing needs to be turned into a Queue, so then locations can be added to queue, and as the enemy reaches the location, dequeue and enqueue a couple locations at a time 

    public void NewTargetLocation(Vector2 moveToTargetLocation)
    {
        // Debug.Log(targetLocation + "target location changed in new target location in move component");
        targetLocation = moveToTargetLocation;
        bHasReachedTarget = false;
        // Debug.DrawLine(aiController.transform.position, moversRigidbody2D.position, Color.red);

    }
    



}
