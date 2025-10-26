
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
//     public int extraJumps = 1;   // how many extra jumps in the air
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

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponentInChildren<Animator>();
//         healthComponentObject = GetComponent<HealthComponent>();
//     }

//     void Update()
//     {
//         if(!healthComponentObject.GetIsKnockedBack()){
//             // Horizontal movement
//             float x = Input.GetAxisRaw("Horizontal");
//             rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
            
//             //flips sprite depending on horizontal input
//             if (x > 0)
//             {
//                 transform.localScale = new Vector3(1, 1, 1);
//             }
//             else if (x < 0)
//             {
//                 transform.localScale = new Vector3(-1, 1, 1);
//             }
    
//             animator.SetBool("isWalking", Mathf.Abs(x) > 0.01f);
//         }
        
//         // Ground check
//         isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

//         if (isGrounded)
//         {
//             jumpsLeft = extraJumps;   // reset extra jumps
//         }

//         // Reset coyote timer if grounded
//         if (isGrounded)
//             coyoteTimeCounter = coyoteTime;
//         else
//             coyoteTimeCounter -= Time.deltaTime;

//         // Check jump input (set buffer timer)
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             jumpBufferCounter = jumpBufferTime;
//         }
//         else
//         {
//             jumpBufferCounter -= Time.deltaTime;
//         }

//         // Jump logic
//         if (jumpBufferCounter > 0f)
//         {
//             // Ground jump (includes coyote time)
//             if (coyoteTimeCounter > 0f)
//             {
//                 DoJump();
//                 jumpBufferCounter = 0f;
//             }
//             // Extra air jumps
//             else if (jumpsLeft > 0)
//             {
//                 DoJump();
//                 jumpsLeft--;
//                 jumpBufferCounter = 0f;
//             }
//         }

//         // Debug.Log("isGrounded: " + isGrounded + " | jumpsLeft: " + jumpsLeft);
//     }

//     void DoJump()
//     {
//         // clear downward velocity before jump
//         rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//         rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

//         coyoteTimeCounter = 0f; // prevent chaining extra coyote jumps
//     }

//     // Visualize ground check box
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

    private Rigidbody2D rb;
    private HealthComponent healthComponentObject;
    private bool isGrounded;
    private Animator animator;

    private bool isAttacking;
    public float attackCooldown = 0.3f; // delay before next attack
    private float attackTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>(); 
        healthComponentObject = GetComponent<HealthComponent>();
    }

    void Update()
    {
        if (!healthComponentObject.GetIsKnockedBack())
        {
            HandleAttack();

            if (!isAttacking)
            {
                HandleMovement();
            }
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

        if (isGrounded)
            jumpsLeft = extraJumps;

        // Update coyote/jump buffer
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

    void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f)
        {
            animator.SetTrigger("attack");
            attackTimer = attackCooldown;
            isAttacking = true;
        }
    }

    //Called from animation event at the start of the attack animation
    public void StartAttack()
    {
        isAttacking = true;
        EnableWeaponHitbox(); 
    }

    //Called from animation event at the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false;
        DisableWeaponHitbox(); 
    }

    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyoteTimeCounter = 0f;
    }

    void EnableWeaponHitbox()
    {
        Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            var col = weapon.GetComponent<BoxCollider2D>();
            if (col != null)
                col.enabled = true;
        }
    }

    void DisableWeaponHitbox()
    {
        Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            var col = weapon.GetComponent<BoxCollider2D>();
            if (col != null)
                col.enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
