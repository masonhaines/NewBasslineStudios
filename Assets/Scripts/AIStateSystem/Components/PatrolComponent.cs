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
    
    private ITarget movementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // private void Start()
    // {
    //     Debug.Log("start called from patrol component");
    //
    //     numberOfActivePatrolPoints = patrolPointLocations.Length;
    //     targetPosition = patrolPointLocations[currentPatrolIndex].position;
    //     movementController.NewTargetLocation(targetPosition);
    // }
    
    private void Awake()
    {
        // Debug.Log("awake called from patrol component");
        movementController = GetComponentInParent<ITarget>(); // reference to all other objects that have implement interface in parent prefab
    }

    public void BeginPatrol()
    {
        if (patrolPointLocations.Length == 0)
        {
            Debug.Log("lost all of my patrol points");
            return;
        }
        
        targetPosition = patrolPointLocations[0].position;
        movementController.NewTargetLocation(targetPosition);
        Debug.Log(patrolPointLocations.Length + " patrol points");
    }
    private IEnumerator SetNewTargetPatrolPoint()
    {
        
        // Debug.Log("set new patrol point called from patrol component");

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
        Debug.Log("on target reached listener called from patrol component");

        if (movementController == null)
        {
            Debug.Log("controller is null");
            return;
        }
        // Debug.Log("Event call from movement has been heard in patrol component");
        Debug.Log("controller is null");
        StartCoroutine(SetNewTargetPatrolPoint());
    }
    
}
