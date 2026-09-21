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

    bool isGrounded;
    bool wall;

    //bool isWallSliding;
    [SerializeField] float wallSlideSpeed = 3f;


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
        wall = false;
        //isWallSliding = false;
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
        Debug.Log(wall);
    }

    void FixedUpdate()
    {
        AnimationManager();
        AdjustGravity();
        WallSlide();
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

    //wallcheck and ground check
    public void SetGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        playerAnimator.SetBool("isGrounded", isGrounded);
        if (isGrounded) jumpCount = 0;

    }

    public void SetWall(bool wall)
    {
        this.wall = wall;
    }

    public void WallSlide()
    {
        if (wall && !isGrounded && moveAction.IsPressed()) 
        {
            //isWallSliding = true; see later for animation
            rb.linearVelocityY = -wallSlideSpeed;
        
        }
        else
        {
            //isWallSliding = false;
        }
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
