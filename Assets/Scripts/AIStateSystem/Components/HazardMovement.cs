using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;
public class HazardMovement : MonoBehaviour
{
     
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float yDistanceToMove = 100f;
    [SerializeField] private float xDistanceToMove = 100f;
    
    private Rigidbody2D moversRigidbody2D;
    private Vector2 targetLocation;
    private Vector2 movementVector;
    
    
    private void Awake()
    {
        moversRigidbody2D = GetComponent<Rigidbody2D>();
        movementVector = new Vector2(xDistanceToMove, yDistanceToMove);
        targetLocation = moversRigidbody2D.position + movementVector;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Moving();
    }

    public void Moving()
    {
        Debug.DrawLine(targetLocation, moversRigidbody2D.position, Color.red);
        

        var distanceTravelled = Vector2.Distance(moversRigidbody2D.position, targetLocation);
        if ( distanceTravelled < 1.5f)
        {
            movementVector = -movementVector;
            targetLocation = moversRigidbody2D.position + movementVector;
        }

   
        Vector2 moveTowardsPosition = Vector2.MoveTowards
        (
            moversRigidbody2D.position, 
            targetLocation, 
            moveSpeed * Time.fixedDeltaTime
        );
        
        
        // where movement is actually happening 
        moversRigidbody2D.MovePosition(moveTowardsPosition);
    }
    
}
