// using UnityEngine;

// public class PlayerController2D : MonoBehaviour
// {
//     [Header("Move")]
//     public float moveSpeed = 6f;

//     [Header("Jump")]
//     public float jumpForce = 12f;
//     public Transform groundCheck;          // drag your GroundCheck here
//     public float groundCheckRadius = 0.15f;
//     public LayerMask groundLayer;          // set to Ground layer in Inspector

//     private Rigidbody2D rb;
//     private bool isGrounded;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     void Update()
//     {
//         // Left/Right 
     
//         float x = Input.GetAxisRaw("Horizontal"); 
//         rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

//         // Ground check 
//         if (groundCheck != null)
//         {
//             isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
//         }

//         //  Jump on Space (only if grounded) 
//         if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//         {
//             // clear any downward speed before jump
//             rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//             rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//         }
//     }

//     // gizmo check or something
//     void OnDrawGizmosSelected()
//     {
//         if (groundCheck == null) return;
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
//     }
// }
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public Transform groundCheck;               // assign child groundDetector
    public Vector2 groundCheckSize = new Vector2(0.6f, 0.1f); // wider, thin box
    public LayerMask groundLayer;

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.15f;            // grace after leaving ground
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.15f;        // grace before landing
    private float jumpBufferCounter;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Reset coyote timer if grounded
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Check jump input (set buffer timer)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Perform jump if within buffer & coyote window
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vertical speed
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Consume both timers
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
        Debug.Log("isGrounded: " + isGrounded);
    }

    // to see ground check box
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}