using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;
public class AiMovementComponent : MonoBehaviour, ITarget
{
    public event System.Action OnTargetReachedCaller = delegate { };
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool bLocalHasMovedToTarget = false;
    [SerializeField] private bool groundOnly = true;
    // public bool bHasReachedTarget { get => bLocalHasMovedToTarget; set => bLocalHasMovedToTarget = value; }
    
    private AIController aiController;
    private Rigidbody2D moversRigidbody2D;
    private Vector2 targetLocation;
    public LayerMask GroundLayer;
    public PolygonCollider2D GroundCollider;
    public SpriteRenderer SpriteRenderer;
    

    private void Awake()
    {
        aiController = GetComponent<AIController>();
        GroundCollider = GetComponent<PolygonCollider2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        moversRigidbody2D = GetComponent<Rigidbody2D>();
        // NewTargetLocation(new Vector2(0, 0)); // init target location 
        Debug.Log(targetLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if (aiController.healthComponentObject.GetIsKnockedBack())
        {
            return;
        }
        if (!bLocalHasMovedToTarget)
        {
            if (!groundOnly || GroundCollider.IsTouchingLayers(GroundLayer))
            {
                Moving();
            }
        }

    }

    public void Moving()
    {
        // throw new System.NotImplementedException();
        // moversRigidbody2D.transform.position = Vector2.MoveTowards(moversRigidbody2D.transform.position, targetLocation, moveSpeed * Time.deltaTime);
        Vector2 moveTowardsPosition = Vector2.MoveTowards(moversRigidbody2D.transform.position, targetLocation, moveSpeed * Time.deltaTime);
        
        // ***** Chat helped with flip lock and delta time toggling
        Vector2 direction = targetLocation - moversRigidbody2D.position;
        
        switch (direction.x)
        {
            case > 0.1f:
                SpriteRenderer.flipX = true; // only flips sprite
                transform.localScale = new Vector3(1, 1, 1);  // face left
                break;
            case < -0.1f:
                // SpriteRenderer.flipX = false; // only flips sprite
                transform.localScale = new Vector3(-1, 1, 1);  // face left
                break;
        }
        
        moversRigidbody2D.MovePosition(moveTowardsPosition);
        
        // if (abs(moversRigidbody2D.transform.position.y - targetLocation.y) >= 0.1f) return;
        // if (Vector2.Distance(moversRigidbody2D.position, targetLocation) <= 0.1f)
        if (Mathf.Abs(moversRigidbody2D.position.x - targetLocation.x) <= 0.1f)
        {
            // Reached the target location
            bLocalHasMovedToTarget = true;
            moversRigidbody2D.position = targetLocation;
            OnTargetReachedCaller.Invoke();
            Debug.Log("Moved to target location");
        }
        
    }


    public void NewTargetLocation(Vector2 moveToTargetLocation)
    {
        targetLocation = moveToTargetLocation;
        bLocalHasMovedToTarget = false;
        Debug.Log(targetLocation);
    }


}
