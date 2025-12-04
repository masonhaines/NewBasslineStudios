using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float InitMoveSpeed;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.18f;
    public float dashCooldown = 0.6f;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    public bool isDashing = false;
    public bool bIsFrozen = false;

    public TrailRenderer trail;

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
    private float attackTimer = 0f;
    private bool isAttacking = false;
    [SerializeField] private Collider2D weapon;

    [Header("Projectile Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileCooldown = 0.5f;
    private float projectileTimer = 0f;

    private Rigidbody2D rb;
    private HealthComponent healthComponentObject;
    public SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded;
    public Color InitColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        healthComponentObject = GetComponent<HealthComponent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        InitColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
        DisableWeaponHitbox();
        InitMoveSpeed = moveSpeed;

        
        if (trail == null)
            trail = GetComponentInChildren<TrailRenderer>();
        if (trail != null)
            trail.emitting = false;

        
        dashCooldownTimer = 0f;
        dashTimer = 0f;
        projectileTimer = 0f;
        attackTimer = 0f;
    }

    void Update()
    {
        if (!healthComponentObject.GetIsKnockedBack())
        {
            HandleDash();
            HandleAttack();
            HandleProjectileAttack();

            
            if (!isAttacking && !isDashing)
                HandleMovement();
        }

        HandleJumping();
    }

    void HandleDash()
    {
        
        dashCooldownTimer -= Time.deltaTime;
        if (bIsFrozen) return; // if status effect active 

        // leftshift to dash
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && !isAttacking)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            
            if (trail != null)
                trail.emitting = true;
        }

        if (isDashing)
        {
            float dir = transform.localScale.x >= 0 ? 1f : -1f;

            //set horizontal velocity to dashSpeed
            rb.linearVelocity = new Vector2(dir * dashSpeed, rb.linearVelocity.y);

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;

                
                if (trail != null)
                    trail.emitting = false;

                // restore moveSpeed
                moveSpeed = InitMoveSpeed;
            }
        }
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

        if (animator != null)
            animator.SetBool("isWalking", Mathf.Abs(x) > 0.01f);
    }

    void HandleJumping()
    {
        if (groundCheck == null)
            return;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // preserve previous behavior: reset extra jumps and update animator
        if (isGrounded)
        {
            jumpsLeft = extraJumps;
            if (animator != null)
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

        if (animator != null)
            animator.SetBool("isJumping", true);
    }

    void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f && !isAttacking && !isDashing)
        {
            if (animator != null)
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

    void HandleProjectileAttack()
    {
        projectileTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && projectileTimer <= 0f)
        {
            if (animator != null)
                animator.SetTrigger("rangedAttack"); 
            projectileTimer = projectileCooldown;
        }
    }

    public void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 shootDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Flip projectile visually if facing left
        if (transform.localScale.x < 0)
            proj.transform.localScale = new Vector3(-1, 1, 1);

        PlayerProjectile projectile = proj.GetComponent<PlayerProjectile>();
        if (projectile != null)
            projectile.Initialize(shootDir, gameObject, rb.linearVelocity.x);
    }

    public void SpawnProjectile()
    {
        FireProjectile();
    }

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
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}