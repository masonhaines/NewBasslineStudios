using UnityEngine;


public class HazPerceptionComponent : MonoBehaviour
{
    
    
    private AiControllerSimple aiController;

    private void Awake()
    {
        aiController = GetComponent<AiControllerSimple>();
    }
    
    private void OnTriggerEnter2D(Collider2D newTarget)
    {
        if (newTarget.CompareTag("Player"))
        {
            aiController.PerceptionTargetFound(newTarget.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D newTarget)
    {
        if (newTarget.CompareTag("Player"))
        {
            aiController.PerceptionTargetLost(newTarget.transform);
        }
    }
}
