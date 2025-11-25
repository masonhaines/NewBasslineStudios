// using UnityEngine;

// public class PlayerController2D : MonoBehaviour
// {
    
    
//     [Header("Movement")]
//     public float moveSpeed = 6f;

//     [Header("Jumping")]
//     public float jumpForce = 12f;
//     public Transform groundCheck;
//     public Vector2 groundCheckSize = new Vector2(0.6f, 0.1f);
//     public LayerMask groundLayer;
//     public int extraJumps = 1;
//     private int jumpsLeft;

//     [Header("Coyote Time & Jump Buffer")]
//     public float coyoteTime = 0.15f;
//     private float coyoteTimeCounter;
//     public float jumpBufferTime = 0.15f;
//     private float jumpBufferCounter;

//     [Header("Attack")]
//     public float attackCooldown = 0.3f; // delay before next attack
//     private float attackTimer;
//     private bool isAttacking;
//     [SerializeField] private Collider2D weapon; // this needs to be given a collider box inside of the editor, from mason 

//     private Rigidbody2D rb;
//     private HealthComponent healthComponentObject;
//     private Animator animator;

//     private bool isGrounded;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponentInChildren<Animator>();
//         healthComponentObject = GetComponent<HealthComponent>();
//         DisableWeaponHitbox();
//     }

//     void Update()
//     {
//         if (!healthComponentObject.GetIsKnockedBack())
//         {
//             HandleAttack();

//             // Disable horizontal movement while attacking (on ground)
//             if (!isAttacking)
//                 HandleMovement();
//         }

//         HandleJumping();
//     }

    
//     void HandleMovement()
//     {
//         float x = Input.GetAxisRaw("Horizontal");
//         rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

//         // Flip sprite
//         if (x > 0)
//             transform.localScale = new Vector3(1, 1, 1);
//         else if (x < 0)
//             transform.localScale = new Vector3(-1, 1, 1);

//         animator.SetBool("isWalking", Mathf.Abs(x) > 0.01f);
//     }

    
//     void HandleJumping()
//     {
//         // Ground check
//         isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

//         // Update jump animation
//         animator.SetBool("isJumping", !isGrounded && rb.linearVelocity.y != 0);

//         if (isGrounded)
//             jumpsLeft = extraJumps;

//         // Coyote & jump buffer timers
//         coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
//         jumpBufferCounter -= Time.deltaTime;

//         if (Input.GetKeyDown(KeyCode.Space))
//             jumpBufferCounter = jumpBufferTime;

//         if (jumpBufferCounter > 0f)
//         {
//             if (coyoteTimeCounter > 0f)
//             {
//                 DoJump();
//                 jumpBufferCounter = 0f;
//             }
//             else if (jumpsLeft > 0)
//             {
//                 DoJump();
//                 jumpsLeft--;
//                 jumpBufferCounter = 0f;
//             }
//         }
//     }

//     void DoJump()
//     {
//         rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//         rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//         coyoteTimeCounter = 0f;
//     }

    
//     void HandleAttack()
//     {
//         // Decrease cooldown timer
//         attackTimer -= Time.deltaTime;

//         // Only allow new attack when cooldown is ready and not currently attacking
//         if (Input.GetMouseButtonDown(0) && attackTimer <= 0f && !isAttacking)
//         {
//             animator.SetTrigger("attack");
//             attackTimer = attackCooldown;
//         }
//     }

//     // Called from animation event at the start of attack animation
//     public void StartAttack()
//     {
//         isAttacking = true;
//         EnableWeaponHitbox();

//         // Stop horizontal movement during attack (only on ground)
//         if (isGrounded)
//             rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
//     }

//     // Called from animation event at the end of attack animation
//     public void EndAttack()
//     {
//         isAttacking = false;
//         DisableWeaponHitbox();
//     }

//     void EnableWeaponHitbox()
//     {
//         // Transform weapon = transform.Find("Weapon");
//         if (weapon != null)
//         {
//             // var col = weapon.GetComponent<BoxCollider2D>();
//             if (weapon != null)
//                 weapon.enabled = true;
//             else 
//                 Debug.Log("was not able to find weapon hit box");
//         }
//         else
//         {
//             Debug.Log("was not able to find weapon hit box");
//         }

//     }

//     void DisableWeaponHitbox()
//     {
//         // Transform weapon = transform.Find("Weapon");
//         if (weapon != null)
//         {
//             // var col = weapon.GetComponent<BoxCollider2D>();
            
//             if (weapon != null)
//                 weapon.enabled = false;
            
//             else 
//                 Debug.Log("was not able to find weapon hit box");
//         }
//         else
//         {
//             Debug.Log("was not able to find weapon hit box");
//         }
//     }


//     void OnDrawGizmosSelected()
//     {
//         if (groundCheck == null) return;
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
//     }
// }
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    public float InitMoveSpeed;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.6f, 0.1f);
    public LayerMask groundLayer;
    public int extraJumps = 1;
    private int jumpsLeft;

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [Header("Melee Attack")]
    public float attackCooldown = 0.3f;
    private float attackTimer;
    private bool isAttacking;
    [SerializeField] private Collider2D weapon;

    [Header("Projectile Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileCooldown = 0.5f;
    private float projectileTimer;

    private Rigidbody2D rb;
    private HealthComponent healthComponentObject;
    private Animator animator;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        healthComponentObject = GetComponent<HealthComponent>();
        DisableWeaponHitbox();
        InitMoveSpeed = moveSpeed;
    }

    void Update()
    {
        // Debug.Log(healthComponentObject.currentHealth);
        if (!healthComponentObject.GetIsKnockedBack())
        {
            HandleAttack();
            HandleProjectileAttack();

            if (!isAttacking)
                HandleMovement();
        }

        HandleJumping();
    }

    // -------------------- Movement --------------------
    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        animator.SetBool("isWalking", Mathf.Abs(x) > 0.01f);
    }

    // -------------------- Jumping --------------------
    void HandleJumping()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        // animator.SetBool("isJumping", !isGrounded && rb.linearVelocity.y != 0);

        // if (isGrounded)
        //     jumpsLeft = extraJumps;
        
        if (isGrounded)
        {
            jumpsLeft = extraJumps;
            animator.SetBool("isJumping", false);
        }

        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
        jumpBufferCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                DoJump();
                jumpBufferCounter = 0f;
            }
            else if (jumpsLeft > 0)
            {
                DoJump();
                jumpsLeft--;
                jumpBufferCounter = 0f;
            }
        }
    }

    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyoteTimeCounter = 0f;
        
        animator.SetBool("isJumping", true);
    }

    // -------------------- Melee Attack --------------------
    void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f && !isAttacking)
        {
            animator.SetTrigger("attack");
            attackTimer = attackCooldown;
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        EnableWeaponHitbox();

        if (isGrounded)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void EndAttack()
    {
        isAttacking = false;
        DisableWeaponHitbox();
    }

    // -------------------- Projectile Attack --------------------
    void HandleProjectileAttack()
{
    projectileTimer -= Time.deltaTime;

    if (Input.GetMouseButtonDown(1) && projectileTimer <= 0f)
    {
        animator.SetTrigger("rangedAttack"); // optional animation
        //FireProjectile();
        projectileTimer = projectileCooldown;
    }
}

public void FireProjectile()
{
    if (projectilePrefab == null || firePoint == null)
    {
        Debug.LogWarning("ProjectilePrefab or FirePoint not assigned!");
        return;
    }

    // Instantiate projectile
    GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

    // Determine shooting direction based on facing
    Vector2 shootDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    // Flip the projectile sprite if facing left
    if (transform.localScale.x < 0)
        proj.transform.localScale = new Vector3(-1, 1, 1);

    // Get the PlayerProjectile script and initialize it properly
    PlayerProjectile projectile = proj.GetComponent<PlayerProjectile>();
    if (projectile != null)
    {
        projectile.Initialize(shootDir, gameObject, rb.linearVelocity.x);
    }
    else
    {
        Debug.LogError("Spawned projectile is missing the PlayerProjectile component!");
    }
}

    // -------------------- Weapon Collider Helpers --------------------
    void EnableWeaponHitbox()
    {
        if (weapon != null)
            weapon.enabled = true;
        else
            Debug.Log("Weapon hitbox not assigned!");
    }

    void DisableWeaponHitbox()
    {
        if (weapon != null)
            weapon.enabled = false;
        else
            Debug.Log("Weapon hitbox not assigned!");
    }

    // -------------------- Debug --------------------
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
