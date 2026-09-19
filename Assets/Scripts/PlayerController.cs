using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    bool isGrounded;
    InputAction jumpAction;

    [SerializeField] float speed = 8f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float gravity = 3f;
    [SerializeField] float fallMult = 2f;
    [SerializeField] float lowJumpMult = 2f;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //circleCollider = GetComponentInChildren<CircleCollider2D>();
        isGrounded = false;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];

    }

    void Start()
    {
        rb.gravityScale = gravity;
        
    }

    void Update()
    {
        if (rb.linearVelocityY == 0f)
        {
            rb.gravityScale = gravity;
        }
        else if (rb.linearVelocityY < 0f)
        {
            rb.gravityScale = gravity * fallMult;
        }
        else if ( rb.linearVelocityY > 0f && !jumpAction.IsPressed())
        {
            rb.gravityScale = gravity * lowJumpMult;
        }

           
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = moveInput.x * speed;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

}
