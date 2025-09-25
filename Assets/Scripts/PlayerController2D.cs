using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public Transform groundCheck;          // drag your GroundCheck here
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;          // set to Ground layer in Inspector

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Left/Right 
     
        float x = Input.GetAxisRaw("Horizontal"); 
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // Ground check 
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        //  Jump on Space (only if grounded) 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // clear any downward speed before jump
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // gizmo check or something
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
