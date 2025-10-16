using System.Collections.Generic;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    private AiMovementComponent movementController;

    private void Awake()
    {
        // moveRef = GetComponentInParent<ITarget>(); // reference to all other objects that have implement interface in parent prefab
        // moveRef = GetComponent<ITarget>() ?? GetComponentInParent<ITarget>();
        movementController = GetComponent<AiMovementComponent>(); // reference to all other objects that have implement interface in parent prefab
    }
    private void Start()
    {
        // numberOfWaypointPositions = waypoints.Count;
        // targetPosition = waypoints[waypointIndex].position;
        // moveRef.NewTargetLocation(targetPosition);
    }

    public void BeginChase(Transform target)
    {
        movementController.NewTargetLocation(target.position);
    }

    public void UpdateChaseLocation(Transform target)
    {
        if (!target) return;
        movementController.NewTargetLocation(target.position);
    }
}


// NullReferenceException: Object reference not set to an instance of an object
// AIController.Update () (at Assets/Scripts/AIStateSystem/AIController.cs:76)

