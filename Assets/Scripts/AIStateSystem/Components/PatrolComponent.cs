using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolComponent : MonoBehaviour
{

    [SerializeField] private Transform[] patrolPointLocations;
    [SerializeField] private float waitTimeBetweenPatrolPoints;
    private int numberOfActivePatrolPoints;
    private Vector2 targetPosition;
    private int currentPatrolIndex = 0;
    
    private ITarget movementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        numberOfActivePatrolPoints = patrolPointLocations.Length;
        targetPosition = patrolPointLocations[currentPatrolIndex].position;
        movementController.NewTargetLocation(targetPosition);
    }
    
    private void Awake()
    {
        movementController = GetComponentInParent<ITarget>(); // reference to all other objects that have implement interface in parent prefab
    }
    
    private IEnumerator SetNewTargetPatrolPoint()
    {
        yield return new WaitForSeconds(waitTimeBetweenPatrolPoints);

        if (currentPatrolIndex < numberOfActivePatrolPoints - 1)
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
    public void OnTargetReachedListener()
    {
        if (movementController == null) return;
        // Debug.Log("Event call from movement has been heard in patrol component");
        StartCoroutine(SetNewTargetPatrolPoint());
    }
    
}
