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
    int jumpCount;

    bool isGrounded;
    bool wall;

    bool isWallSliding;
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.2f;
    float wallJumpTimer;
    Vector2 wallJumpSpeed = new Vector2(8f, 16f);
    [SerializeField] float wallSlideSpeed = 4f;
    


    [SerializeField] float speed = 8f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravity = 4f;
    [SerializeField] float fallMult = 2f;
    [SerializeField] float lowJumpMult = 2f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playertransform = GetComponent<Transform>();
      
        isGrounded = false;
        wall = false;
        isWallSliding = false;
        isWallJumping = false;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        moveAction = GetComponent<PlayerInput>().actions["Move"];

    }

    void Start()
    {
        rb.gravityScale = gravity;
        jumpCount = 0;

    }


    void FixedUpdate()
    {
        AnimationManager();
        AdjustGravity();
        WallSlide();
        Move();
        Flip();

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        
    }

    void Flip()
    {
        if ( !isWallJumping)
        {
            if (moveInput.x > 0)
            {
                playertransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (moveInput.x < 0)
            {

                playertransform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        
    }

    // move

    private void Move()
    {
     

        if (!isWallJumping) { 
            rb.linearVelocityX = moveInput.x * speed;
        }
    }

    void OnJump(InputValue value)
    {
        if  (isWallSliding && wallJumpTimer > 0f)
        {
            rb.linearVelocityY = wallJumpSpeed.y;
            rb.linearVelocityX = wallJumpSpeed.x * wallJumpDirection;
            isWallJumping = true;
            playerAnimator.SetTrigger("jumping");
            jumpCount = 1;

            if (wallJumpDirection < 0)
            {
                playertransform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (wallJumpDirection > 0)
            {
                playertransform.rotation = Quaternion.Euler(0, 0, 0);
            }

            
        }
        else if ( !isWallJumping && (isGrounded || jumpCount < maxJump))
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
        else if (rb.linearVelocityY > 0f)
        {
            if (!jumpAction.IsPressed()) { rb.gravityScale = gravity * lowJumpMult; }
            else { rb.gravityScale = gravity; }
            
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

    void WallSlide()
    {
        if (wall && !isGrounded && moveAction.IsPressed() && !isWallJumping) 
        {
            isWallSliding = true; //see later for animation
            rb.linearVelocityY = -wallSlideSpeed;

            //for walljump
            wallJumpDirection = -moveInput.x;
            wallJumpTimer = wallJumpTime;
            
            
        
        }
        else
        {

            if (wallJumpTimer <= 0f) 
            {
                isWallJumping = false;
            }
            else if (wallJumpTimer > 0f)
            {
                wallJumpTimer -= Time.fixedDeltaTime;
            }

            isWallSliding = false;

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
