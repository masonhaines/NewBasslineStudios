using System.Collections.Generic;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    private List<Transform> waypoints;
    private int numberOfWaypointPositions;
    private int waypointIndex = 0;
    private Vector2 targetPosition;


    private ITarget moveRef;

    private void Awake()
    {
        moveRef = GetComponentInParent<ITarget>(); // reference to all other objects that have implement interface in parent prefab
    }
    private void Start()
    {
        // numberOfWaypointPositions = waypoints.Count;
        targetPosition = waypoints[waypointIndex].position;
        moveRef.NewTargetLocation(targetPosition);
    }

    public void OnTargetReachedListener()
    {
        if(moveRef == null) return;
        targetPosition = waypoints[0].position;
        moveRef.NewTargetLocation(targetPosition);
        waypoints.RemoveAt(0);
    }
    
    // call me in the update of ai controller
    public void GetNewWaypoint(Transform targetWaypoint)
    {
        waypoints.Add(targetWaypoint);
    }
    
    

}


// NullReferenceException: Object reference not set to an instance of an object
// AIController.Update () (at Assets/Scripts/AIStateSystem/AIController.cs:76)

