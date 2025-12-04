// using System.Collections;
// using UnityEngine;

// public class RespawnManager : MonoBehaviour
// {
//     [Header("Refs")]
//     [SerializeField] private Transform player;
//     [SerializeField] private Transform spawnPoint;

//     [Header("Tuning")]
//     [SerializeField] private float respawnDelay = 0.75f;

//     // Optional: drag your movement script here if you want it disabled while “dead”
//     private PlayerController2D playerMovement; // e.g., PlayerController
//     private HealthComponent playerHealth;
//     private Rigidbody2D rb2D;
//     private Rigidbody rb3D;
//     private float savedGravityScale;

//     void Awake()
//     {
//         if (player != null)
//         {
//             rb2D = player.GetComponent<Rigidbody2D>();
//             rb3D = player.GetComponent<Rigidbody>();
//             playerHealth = player.GetComponent<HealthComponent>();
//             playerMovement = player.GetComponent<PlayerController2D>();
//             savedGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
//         }
//     }

//     void OnEnable()
//     {
//         if (playerHealth != null)
//             playerHealth.OnDeathCaller += HandlePlayerDeath;
//     }

//     void OnDisable()
//     {
//         if (playerHealth != null)
//             playerHealth.OnDeathCaller -= HandlePlayerDeath;
//     }

//     private void HandlePlayerDeath()
//     {
//         StartCoroutine(RespawnRoutine());
//     }

//     private IEnumerator RespawnRoutine()
//     {
//         // freeze input while “dead” so we dont move after death
//         if (playerMovement) playerMovement.enabled = false;

//         // small delay for death animation/sfx this should mathc whatver u have for animation 
//         yield return new WaitForSeconds(respawnDelay);

//         // ensure game not paused
//         Time.timeScale = 1f;

//         // teleport to spawn
//         if (spawnPoint && player)
//             player.position = spawnPoint.position;

//         // zero velocity
//         if (rb2D) rb2D.linearVelocity = Vector2.zero;
//         if (rb3D) rb3D.linearVelocity = Vector3.zero;

//         // refill health so we can respawn with full
//         if (playerHealth != null)
//             playerHealth.RestoreFullHealth();

//         // re-enable input
//         if (playerMovement) playerMovement.enabled = true;
        
//         player.GetComponent<Rigidbody2D>().gravityScale = savedGravityScale;
//         playerMovement.moveSpeed = playerMovement.InitMoveSpeed;
//         playerMovement.spriteRenderer.color = playerMovement.InitColor;
//     }
// }


using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private HealthComponent playerHealth;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform player;
    private PlayerController2D playerMovement;
    private float savedGravityScale;

    [Header("Death UI")]
    [SerializeField] private CanvasGroup deathBanner; // CanvasGroup on the YOU HAVE FALLEN scroll
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private float holdTime = 1.0f;

    private void Awake()
    {
        if (playerHealth != null)
        {
            // Subscribe to the player's death event
            playerHealth.OnDeathCaller += OnPlayerDeath;
        }

        if (deathBanner != null)
        {
            deathBanner.alpha = 0f;
            deathBanner.gameObject.SetActive(false);
        }
        
        playerMovement = player.GetComponent<PlayerController2D>();
        savedGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeathCaller -= OnPlayerDeath;
        }
    }

    private void OnPlayerDeath()
    {
        // Start the death sequence (show banner, then respawn)
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // --- Fade IN banner ---
        if (deathBanner != null)
        {
            deathBanner.gameObject.SetActive(true);
            float t = 0f;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                deathBanner.alpha = Mathf.Clamp01(t / fadeTime);
                yield return null;
            }
        }

        // Hold on screen
        yield return new WaitForSeconds(holdTime);

        // --- Respawn player ---
        if (respawnPoint != null && playerHealth != null)
        {
            playerHealth.transform.position = respawnPoint.position;
            playerHealth.RestoreFullHealth();   // this will also refill your hearts UI
            player.GetComponent<Rigidbody2D>().gravityScale = savedGravityScale;
         playerMovement.moveSpeed = playerMovement.InitMoveSpeed;
         playerMovement.spriteRenderer.color = playerMovement.InitColor;
        }

        // --- Fade OUT banner ---
        if (deathBanner != null)
        {
            float t = fadeTime;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                deathBanner.alpha = Mathf.Clamp01(t / fadeTime);
                yield return null;
            }
            deathBanner.gameObject.SetActive(false);
        }
    }
}
