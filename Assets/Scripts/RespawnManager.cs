using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private HealthComponent playerHealth;
    [SerializeField] private Transform spawnPoint;

    [Header("Tuning")]
    [SerializeField] private float respawnDelay = 0.75f;

    // Optional: drag your movement script here if you want it disabled while “dead”
    [SerializeField] private MonoBehaviour playerMovement; // e.g., PlayerController

    private Rigidbody2D rb2D;
    private Rigidbody rb3D;

    void Awake()
    {
        if (player != null)
        {
            rb2D = player.GetComponent<Rigidbody2D>();
            rb3D = player.GetComponent<Rigidbody>();
        }
    }

    void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnDeathCaller += HandlePlayerDeath;
    }

    void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDeathCaller -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        // freeze input while “dead”
        if (playerMovement) playerMovement.enabled = false;

        // small delay for death animation/sfx
        yield return new WaitForSeconds(respawnDelay);

        // ensure game not paused
        Time.timeScale = 1f;

        // teleport to spawn
        if (spawnPoint && player)
            player.position = spawnPoint.position;

        // zero velocity
        if (rb2D) rb2D.linearVelocity = Vector2.zero;
        if (rb3D) rb3D.linearVelocity = Vector3.zero;

        // refill health (helper you added)
        if (playerHealth != null)
            playerHealth.RestoreFullHealth();

        // re-enable input
        if (playerMovement) playerMovement.enabled = true;
    }
}
