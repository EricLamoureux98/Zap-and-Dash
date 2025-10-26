using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Range(1f, 0f)]
    [SerializeField] float groundFriction = 0.9f;

    Rigidbody2D rb;
    Animator anim;
    Vector2 moveInput;
    Vector2 velocity;
    bool grounded;
    float facingDirection = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("isJumping", !grounded); 
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
        if (grounded && moveInput.x == 0)
        {
            velocity.x *= groundFriction;
        }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapArea(groundCheck.bounds.min, groundCheck.bounds.max, groundMask);
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Move(InputAction.CallbackContext context)
    {
        anim.SetBool("isRunning", true);

        if (context.canceled)
        {
            anim.SetBool("isRunning", false);
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
