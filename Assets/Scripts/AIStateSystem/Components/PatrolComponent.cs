using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolComponent : MonoBehaviour
{

    [SerializeField] private Transform[] patrolPointLocations;
    [SerializeField] private float waitTimeBetweenPatrolPoints;
    // private int numberOfActivePatrolPoints;
    private Vector2 targetPosition;
    private int currentPatrolIndex = 0;
    private AiMovementComponent movementController;

    
    private void Awake()
    {
        movementController = GetComponent<AiMovementComponent>(); // reference to all other objects that have implement interface in parent prefab
    }

    public void BeginPatrol()
    {
        if (patrolPointLocations.Length == 0)
        {
            patrolPointLocations = new Transform[1]; // make patrol points of size one
            patrolPointLocations[0] = transform; // set self transform as patrol point 
        }

        if (currentPatrolIndex >= patrolPointLocations.Length)
        {
            currentPatrolIndex = 0; // safety check
        }
        targetPosition = patrolPointLocations[currentPatrolIndex].position;
        movementController.NewTargetLocation(targetPosition);
    }
    private IEnumerator SetNewTargetPatrolPoint()
    {

        if (patrolPointLocations.Length == 0)
        {
            Debug.Log("lost all of my patrol points");
            yield break;
        }
        
        yield return new WaitForSeconds(waitTimeBetweenPatrolPoints);

        if (currentPatrolIndex < patrolPointLocations.Length - 1)
        {
            // Debug.Log($"Current Patrol Index: {currentPatrolIndex}");
            currentPatrolIndex++;
        }
        else
        {
            currentPatrolIndex = 0;
        }

        targetPosition = patrolPointLocations[currentPatrolIndex].position;
        movementController.NewTargetLocation(targetPosition);
    }

    // https://docs.unity3d.com/6000.2/Documentation/ScriptReference/WaitForSeconds.html
    public void OnTargetReachedListener() // this is called from the movement component being used
    {
        if (movementController == null)
        {
            Debug.Log("controller is null");
            return;
        }

        StartCoroutine(SetNewTargetPatrolPoint());
    }
    
}
