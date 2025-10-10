using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;
public class HazardMovement : MonoBehaviour
{
     
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float yDistanceToMove = 100f;
    [SerializeField] private float xDistanceToMove = 100f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float waveAmplitude = 1.5f;
    [SerializeField] private bool moveStraight;
    
    private Rigidbody2D moversRigidbody2D;
    private Vector2 targetLocation;
    private Vector2 movementVector;
    private Vector2 activeTargetLocation;
    
    
    private void Awake()
    {
        moversRigidbody2D = GetComponent<Rigidbody2D>();
        movementVector = new Vector2(xDistanceToMove, yDistanceToMove);
        targetLocation = moversRigidbody2D.position + movementVector;
    }

    // Update is called once per frame
    public void Update()
    {
        Moving();
    }

    public void Moving()
    {
        Debug.DrawLine(targetLocation, moversRigidbody2D.position, Color.red);
        
        if (moveStraight)
        {
            var distanceTravelled = Vector2.Distance(moversRigidbody2D.position, targetLocation);
            if ( distanceTravelled < 1.5f)
            {
                movementVector = -movementVector;
                targetLocation = moversRigidbody2D.position + movementVector;
            }

            float offsetY = Mathf.Sin(Time.fixedDeltaTime * moveSpeed) * waveAmplitude;
            activeTargetLocation = targetLocation + new Vector2(0, offsetY);
        }
        else
        {
            var xCos = Mathf.Cos(Time.fixedDeltaTime * moveSpeed) * radius;
            var ySin = Mathf.Sin(Time.fixedDeltaTime * moveSpeed) * radius;
            var waveVector = new Vector2(xCos * xDistanceToMove, ySin * yDistanceToMove);
            
            targetLocation = moversRigidbody2D.position + movementVector + waveVector;
            activeTargetLocation = targetLocation;
        }
        
        Vector2 moveTowardsPosition = Vector2.MoveTowards
        (
            moversRigidbody2D.position, 
            activeTargetLocation, 
            moveSpeed * Time.fixedDeltaTime
        );
        
        Vector2 direction = targetLocation - moversRigidbody2D.position;
        
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
    
}
