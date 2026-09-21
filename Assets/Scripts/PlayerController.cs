using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    
    InputAction jumpAction;
    InputAction moveAction;
    Animator playerAnimator;
    Transform playertransform;

    int maxJump = 2;
    public int jumpCount;

    public bool isGrounded;
    public bool wall;


    [SerializeField] float speed = 8f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravity = 4f;
    [SerializeField] float fallMult = 2f;
    [SerializeField] float lowJumpMult = 2f;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] WallCheck wallCheck;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playertransform = GetComponent<Transform>();
      
        isGrounded = false;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        moveAction = GetComponent<PlayerInput>().actions["Move"];

    }

    void Start()
    {
        rb.gravityScale = gravity;
        jumpCount = 0;

    }

    private void Update()
    {
        AnimationManager();
        Debug.Log(wall);
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
            playertransform.rotation = Quaternion.Euler(0, 0,0 );
        }
        else if ( moveInput.x < 0) 
        {

            playertransform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void OnJump(InputValue value)
    {

        if (isGrounded || jumpCount < maxJump )
        {
            rb.linearVelocityY = jumpForce;
            playerAnimator.SetTrigger("jumping");
            jumpCount++;
        }

    }

    //TRIGGER



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


    public void SetGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        playerAnimator.SetBool("isGrounded", isGrounded);
        if (isGrounded) jumpCount = 0;

    }

    void AnimationManager()
    {
        playerAnimator.SetFloat("velocityY", rb.linearVelocityY);

        if (moveAction.IsPressed() && isGrounded)
        {
            playerAnimator.SetBool("MoveAction", true);
        }
        else { playerAnimator.SetBool("MoveAction", false); }

    }

}
