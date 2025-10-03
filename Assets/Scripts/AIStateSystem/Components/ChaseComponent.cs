using System.Collections.Generic;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    // private List<Transform> waypoints;
    // private int numberOfWaypointPositions;
    // private Vector2 targetPosition;


    private ITarget movementController;

    private void Awake()
    {
        // moveRef = GetComponentInParent<ITarget>(); // reference to all other objects that have implement interface in parent prefab
        // moveRef = GetComponent<ITarget>() ?? GetComponentInParent<ITarget>();
        movementController = GetComponent<ITarget>(); // reference to all other objects that have implement interface in parent prefab
        // waypoints = new List<Transform>();
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

    // public void OnTargetReachedListener()
    // {
    //     if(movementController == null) return;
    //     targetPosition = waypoints[0].position;
    //     movementController.NewTargetLocation(targetPosition);
    //     waypoints.RemoveAt(0);
    // }
    //
    // // call me in the update of ai controller
    // public void GetNewWaypoint(Transform targetWaypoint)
    // {
    //     if (!targetWaypoint) return;
    //     waypoints.Add(targetWaypoint);
    //     
    // }
}


// NullReferenceException: Object reference not set to an instance of an object
// AIController.Update () (at Assets/Scripts/AIStateSystem/AIController.cs:76)

