using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsSimpleUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private HealthComponent playerHealth;
    [SerializeField] private List<Image> heartImages = new List<Image>();

    [Header("Setup")]
    [Tooltip("Leave 0 to auto-use the number of child Images")]
    [SerializeField] private int startingHearts = 0;

    private int heartsRemaining;

    private void Reset()
    {
        // Auto-find the player's HealthComponent in the scene
#if UNITY_2023_1_OR_NEWER
        if (playerHealth == null) playerHealth = FindAnyObjectByType<HealthComponent>();
#else
        if (playerHealth == null) playerHealth = FindObjectOfType<HealthComponent>();
#endif
        // Auto-collect direct child Images (left→right)
        if (heartImages.Count == 0)
        {
            foreach (Transform child in transform)
            {
                var img = child.GetComponent<Image>();
                if (img != null) heartImages.Add(img);
            }
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHitCaller += OnHitOnce;   // one heart per hit
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHitCaller -= OnHitOnce;
    }

    private void Start()
    {
        heartsRemaining = (startingHearts > 0)
            ? Mathf.Min(startingHearts, heartImages.Count)
            : heartImages.Count;  // default: one heart per icon
        Refresh();
    }

    private void OnHitOnce(Transform _)
    {
        heartsRemaining = Mathf.Max(0, heartsRemaining - 1);
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < heartImages.Count; i++)
            heartImages[i].enabled = (i < heartsRemaining); // disappear one by one
    }
}
