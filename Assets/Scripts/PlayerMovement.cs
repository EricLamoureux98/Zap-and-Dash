using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] BoxCollider2D groundCheck;

    [Header("Player Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [Range(1f, 0f)] [SerializeField] float groundFriction = 0.9f;

    public bool grounded { get; private set; }
    public bool isMoving { get; private set; }

    Rigidbody2D rb;
    Animator anim;

    Vector2 moveInput;
    Vector2 velocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Not proper but prevents animation from playing on ledges. 
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            anim.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        CheckGround();
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

    void CheckGround()
    {
        grounded = Physics2D.OverlapArea(groundCheck.bounds.min, groundCheck.bounds.max, groundMask);
        anim.SetBool("isGrounded", grounded);
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("isJumping", true);
        }
    }
}
