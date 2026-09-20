using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    bool isGrounded;
    InputAction jumpAction;
    InputAction moveAction;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool faceRight;

    [SerializeField] float speed = 8f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravity = 4f;
    [SerializeField] float fallMult = 2f;
    [SerializeField] float lowJumpMult = 2f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
      
        isGrounded = false;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        moveAction = GetComponent<PlayerInput>().actions["Move"];

    }

    void Start()
    {
        rb.gravityScale = gravity;

    }

    private void Update()
    {
        AnimationManager();
    }

    void FixedUpdate()
    {
        AdjustGravity();
        rb.linearVelocityX = moveInput.x * speed;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if ( moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if ( moveInput.x < 0) 
        {
        
            spriteRenderer.flipX = true;
        }
    }

    void OnJump(InputValue value)
    {

        if (isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    private void AdjustGravity()
    {
        if (isGrounded)
        {
            rb.gravityScale = gravity;
        }
        else if (rb.linearVelocityY < 0f)
        {
            rb.gravityScale = gravity * fallMult;
            
        }
        else if (rb.linearVelocityY > 0f && !jumpAction.IsPressed())
        {
            rb.gravityScale = gravity * lowJumpMult;
        }
    }

    void AnimationManager()
    {
        animator.SetFloat("velocityY", rb.linearVelocityY);

        if (moveAction.IsPressed() && isGrounded)
        {
            animator.SetBool("MoveAction", true);
        }
        else { animator.SetBool("MoveAction", false); }

    }

}
