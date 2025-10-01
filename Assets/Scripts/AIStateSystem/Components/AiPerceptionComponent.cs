using UnityEngine;

// this component needs to be on a child object with its own trigger collision box and its own perception layer
// in the 2d collision matrix it needs to be ignored by the damage collision boxes and needs to ignore enemy layer as well 
// MAKE SURE PLAYER HAS PLAYER TAG
public class AiPerceptionComponent : MonoBehaviour
{
    // [SerializeField] private int distanceFromTarget;
    
    private AIController aiController;

    private void Awake()
    {
        aiController = GetComponentInParent<AIController>();
    }
    
    private void OnTriggerEnter2D(Collider2D newTarget)
    {
        if (newTarget.CompareTag("Player"))
        {
            aiController.PerceptionTargetFound(newTarget.transform);
            Debug.Log("perception - Player found");
        }
    }

    private void OnTriggerExit2D(Collider2D newTarget)
    {
        if (newTarget.CompareTag("Player"))
        {
            aiController.PerceptionTargetLost(newTarget.transform);
            Debug.Log("perception - Player lost");
        }
    }
}
