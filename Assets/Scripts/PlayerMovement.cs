using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;

    [Header("Player Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float variableJump = 0.5f;
    [Range(1f, 0f)][SerializeField] float groundFriction = 0.9f;

    [Header("Collider Info")]
    [SerializeField] Vector2 jumpColliderSize = new Vector2(1f, 1f);
    [SerializeField] Vector2 jumpColliderOffset = new Vector2(0f, 0.65f);
    
    [Header("Knockback")]
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float stunTime = 0.5f;
    [SerializeField] float knockbackTime = 0.5f;
    bool isKnockedback;

    public bool grounded { get; private set; }
    public bool isMoving { get; private set; }

    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D playerCollider;

    Vector2 moveInput;
    Vector2 velocity;
    Vector2 normalColliderSize;
    Vector2 normalColliderOffset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        normalColliderSize = playerCollider.size;
        normalColliderOffset = playerCollider.offset;
    }

    void Update()
    {
        grounded = CheckGround();

        if (grounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f && playerCollider.size != normalColliderSize)
        {
            RestoreCollider();
            anim.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();        
        ApplyFriction();   

        // Checks direction player is facing in contrast to movement direction
        if (moveInput.x > 0 && transform.localScale.x < 0 || moveInput.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
        
    }

    void MovePlayer()
    {
        if (isKnockedback) return;

        velocity = rb.linearVelocity;
        velocity.x = moveInput.x * moveSpeed;
        rb.linearVelocity = velocity;
    }

    void ApplyFriction()
    {
        if (grounded && moveInput.x == 0 && rb.linearVelocity.y <= 0)
        {
            velocity.x *= groundFriction;
        }
    }

    bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 1f, groundMask);
        //anim.SetBool("isGrounded", grounded);
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void RestoreCollider()
    {
        playerCollider.size = normalColliderSize;
        playerCollider.offset = normalColliderOffset;
        transform.position += new Vector3(0, 1.75f, 0);
    }

    public void KnockBack(Transform shooterPosition)
    {
        isKnockedback = true;
        StartCoroutine(StunTimer());
        Vector2 direction = (transform.position - shooterPosition.position).normalized;
        Vector2 force = knockbackForce * direction;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        isKnockedback = false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        anim.SetBool("isRunning", true);
        isMoving = true;

        if (context.canceled)
        {
            anim.SetBool("isRunning", false);
            isMoving = false;
        }

        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            playerCollider.size = jumpColliderSize;
            playerCollider.offset = jumpColliderOffset;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("isJumping", true);
        }
        if (context.canceled && rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * variableJump);
        }
    }
}
