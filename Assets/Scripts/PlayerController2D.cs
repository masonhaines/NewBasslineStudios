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

//     private Rigidbody2D rb;
//     private HealthComponent healthComponentObject;
//     private bool isGrounded;
//     private Animator animator;

//     private bool isAttacking;
//     public float attackCooldown = 0.3f; // delay before next attack
//     private float attackTimer;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponentInChildren<Animator>(); 
//         healthComponentObject = GetComponent<HealthComponent>();
//     }

//     void Update()
//     {
//         if (!healthComponentObject.GetIsKnockedBack())
//         {
//             HandleAttack();

//             if (!isAttacking)
//             {
//                 HandleMovement();
//             }
            
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
// {
//     // Ground check
//     isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

//     // Update jump animation
//     animator.SetBool("isJumping", !isGrounded && rb.linearVelocity.y != 0);

//     if (isGrounded)
//         jumpsLeft = extraJumps;

//     // Update coyote/jump buffer
//     coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
//     jumpBufferCounter -= Time.deltaTime;

//     if (Input.GetKeyDown(KeyCode.Space))
//         jumpBufferCounter = jumpBufferTime;

//     if (jumpBufferCounter > 0f)
//     {
//         if (coyoteTimeCounter > 0f)
//         {
//             DoJump();
//             jumpBufferCounter = 0f;
//         }
//         else if (jumpsLeft > 0)
//         {
//             DoJump();
//             jumpsLeft--;
//             jumpBufferCounter = 0f;
//         }
//     }
// }

//     void HandleAttack()
//     {
//         attackTimer -= Time.deltaTime;

//         if (Input.GetMouseButtonDown(0) && attackTimer <= 0f)
//         {
//             isAttacking = true;
//             animator.SetTrigger("attack");
//             attackTimer = attackCooldown;
            
//         }
//     }

//     //Called from animation event at the start of the attack animation
//     public void StartAttack()
//     {
//         isAttacking = true;
//         EnableWeaponHitbox();
//         if (isGrounded)//stops player from moving while attacking on ground but allows for movement while attacking in air
//         {
//             rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
//         }
        
//     }

//     //Called from animation event at the end of the attack animation
//     public void EndAttack()
//     {
//         float x = Input.GetAxisRaw("Horizontal");
//         isAttacking = false;
//         DisableWeaponHitbox(); 
//         rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);//allows player to continue moving after attack on ground without having to press button again
//     }

//     void DoJump()
//     {
//         rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//         rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//         coyoteTimeCounter = 0f;
//     }

//     void EnableWeaponHitbox()
//     {
//         Transform weapon = transform.Find("Weapon");
//         if (weapon != null)
//         {
//             var col = weapon.GetComponent<BoxCollider2D>();
//             if (col != null)
//                 col.enabled = true;
//         }
//     }

//     void DisableWeaponHitbox()
//     {
//         Transform weapon = transform.Find("Weapon");
//         if (weapon != null)
//         {
//             var col = weapon.GetComponent<BoxCollider2D>();
//             if (col != null)
//                 col.enabled = false;
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

public class PlayerController2D : MonoBehaviour
{
    
    
    [Header("Movement")]
    public float moveSpeed = 6f;

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

    [Header("Attack")]
    public float attackCooldown = 0.3f; // delay before next attack
    private float attackTimer;
    private bool isAttacking;
    [SerializeField] private Collider2D weapon; // this needs to be given a collider box inside of the editor, from mason 

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
    }

    void Update()
    {
        if (!healthComponentObject.GetIsKnockedBack())
        {
            HandleAttack();

            // Disable horizontal movement while attacking (on ground)
            if (!isAttacking)
                HandleMovement();
        }

        HandleJumping();
    }

    
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

    
    void HandleJumping()
    {
        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Update jump animation
        animator.SetBool("isJumping", !isGrounded && rb.linearVelocity.y != 0);

        if (isGrounded)
            jumpsLeft = extraJumps;

        // Coyote & jump buffer timers
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
    }

    
    void HandleAttack()
    {
        // Decrease cooldown timer
        attackTimer -= Time.deltaTime;

        // Only allow new attack when cooldown is ready and not currently attacking
        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f && !isAttacking)
        {
            animator.SetTrigger("attack");
            attackTimer = attackCooldown;
        }
    }

    // Called from animation event at the start of attack animation
    public void StartAttack()
    {
        isAttacking = true;
        EnableWeaponHitbox();

        // Stop horizontal movement during attack (only on ground)
        if (isGrounded)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    // Called from animation event at the end of attack animation
    public void EndAttack()
    {
        isAttacking = false;
        DisableWeaponHitbox();
    }

    void EnableWeaponHitbox()
    {
        // Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            // var col = weapon.GetComponent<BoxCollider2D>();
            if (weapon != null)
                weapon.enabled = true;
            else 
                Debug.Log("was not able to find weapon hit box");
        }
        else
        {
            Debug.Log("was not able to find weapon hit box");
        }

    }

    void DisableWeaponHitbox()
    {
        // Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            // var col = weapon.GetComponent<BoxCollider2D>();
            
            if (weapon != null)
                weapon.enabled = false;
            
            else 
                Debug.Log("was not able to find weapon hit box");
        }
        else
        {
            Debug.Log("was not able to find weapon hit box");
        }
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}