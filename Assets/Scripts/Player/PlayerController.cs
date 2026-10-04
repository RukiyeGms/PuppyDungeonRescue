using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


//PlayerMovement
public class PlayerController : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D rb;
    
    InputAction jumpAction;
    InputAction moveAction;
    Animator playerAnimator;
    public Transform playertransform;
    ParticleSystem particleS;

    int maxJump = 2;
    int jumpCount;

    bool isGrounded;
    bool wall;

    //walljump
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

    //knockback
    [SerializeField] float knockBackForceX = 15f;
    [SerializeField] float knockBackForceY = 15f;
    [SerializeField] float knockBackTime = 0.5f;
    public bool knockBack;

    bool dead;
    public bool isAttacking;

    IInterractable currentInterractable;

    //resopawn
    public Transform respawnTransform;

    bool stopMove;

    //SFX
    float stepSpoundSpeed = 0.35f;
    bool playingFootsteps;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playertransform = GetComponent<Transform>();
        particleS = GetComponentInChildren<ParticleSystem>();
      
        isGrounded = false;
        wall = false;
        isWallSliding = false;
        isWallJumping = false;
        knockBack = false;
        stopMove = false;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        moveAction = GetComponent<PlayerInput>().actions["Move"];

        //handle death
        dead = false;
        PlayerHealth.OnPuppyDied += Death;

        currentInterractable = null;

        playingFootsteps = false;


    }

    void Start()
    {
        rb.gravityScale = gravity;
        jumpCount = 0;
    }


    void FixedUpdate()
    {
        AnimationManager();
        SFXManager();
        AdjustGravity();
        if (!knockBack && !dead && !isAttacking && !stopMove)
        {
            WallSlide();
            Move();
            Flip();
        }
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

    private void Move()
    {

        if (!isWallJumping && !stopMove) { 
            rb.linearVelocityX = moveInput.x * speed;
        }
    }

    void OnJump(InputValue value)
    {
        if (!stopMove) 
        {
            if (isWallSliding && wallJumpTimer > 0f && !knockBack)
            {
                rb.linearVelocityY = wallJumpSpeed.y;
                rb.linearVelocityX = wallJumpSpeed.x * wallJumpDirection;
                isWallJumping = true;
                JumpingFX();
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
            else if (!isWallJumping && (isGrounded || jumpCount < maxJump) && !knockBack)
            {
                rb.linearVelocityY = jumpForce;
                JumpingFX();
                jumpCount++;


            }
        }
        

    }

    void JumpingFX()
    {
        playerAnimator.SetTrigger("jumping");
        particleS.Play();
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
        else if (rb.linearVelocityY > 0f  && !knockBack)
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
        else { jumpCount = 1; }

    }

    public void SetWall(bool wall)
    {
        this.wall = wall;
    }

    void WallSlide()
    {
        if (wall && !isGrounded && moveAction.IsPressed() && !isWallJumping) 
        {
            isWallSliding = true;

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

    //Combat

    public void KnockBackRoutine(bool playerOnRight)
    {
        StartCoroutine(GetKnockedBack(playerOnRight));
    }

    public IEnumerator GetKnockedBack(bool playerOnRight)
    {
        knockBack = true;
        rb.linearVelocityY = knockBackForceY;
        if (playerOnRight) 
        {
            rb.linearVelocityX = knockBackForceX;
            playertransform.rotation = Quaternion.Euler(0, 180 ,0);
        }
        else 
        { 
            rb.linearVelocityX = -knockBackForceX;
            playertransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        yield return new WaitForSeconds(knockBackTime);
        knockBack = false;
        
    }

    void Death()
    {
        dead = true;
    }



    void AnimationManager()
    {
        //death animation
        playerAnimator.SetBool("ouch", knockBack);
        

        if (!knockBack) { playerAnimator.SetFloat("velocityY", rb.linearVelocityY); }
        else { playerAnimator.SetFloat("velocityY", 0); }

        if (moveAction.IsPressed() && isGrounded && !stopMove)
        {
            playerAnimator.SetBool("MoveAction", true);
            //walkingsound play
        }
        else 
        { 
            playerAnimator.SetBool("MoveAction", false); 
            //walking sound stop
        }

    }

    void SFXManager()
    {
        if (moveAction.IsPressed() && isGrounded && !stopMove)
        {
            if (!playingFootsteps) 
            {
                playingFootsteps = true;
                InvokeRepeating(nameof(PlayStepSound), 0, stepSpoundSpeed);
            }
            
            //walkingsound play
        }
        else
        {
            playingFootsteps = false;
            CancelInvoke(nameof(PlayStepSound));
            //walking sound stop
        }

    }

    //footstepsounds

    void PlayStepSound()
    {
        SoundEffectManager.Play("PuppyWalk");
    }

    public void ChangeInterractable(IInterractable interactable)
    {
        currentInterractable = interactable;
    }

    public void InterractableNull() { currentInterractable = null; }

    void OnInterract()
    {
        if (currentInterractable != null) { currentInterractable.Interract(); }
        
    }

    public void PlayeCanMove(bool canMove)
    {
        stopMove = !canMove;
        if (!canMove)
        {
            rb.linearVelocityX = 0;
        }
    }

    void OnDestroy()
    {
        PlayerHealth.OnPuppyDied -= Death;
    }

}
