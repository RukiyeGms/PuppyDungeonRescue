using UnityEngine;

public class TeethEnemy : MonoBehaviour
{
    Vector2 playerPosition;
    Animator anim;
    [SerializeField] Animator emoteAnim;
    bool isTailing;
    bool isAttacking;


    [SerializeField] EnemyVision enemyVision;
    [SerializeField] EnemySidePatrol enemySidePatrol;
    Rigidbody2D rb;

    [SerializeField] float followSpeed = 6f;
    public float followTime = 1.0f;
    float followTimer;

    float readyAttackTimer;
    [SerializeField] float readyAttackTime = 0.5f;

    float attackCoolTimeTimer;
    [SerializeField] float attackCoolTime = 0.5f;

    float attackedTimer;
    [SerializeField] float attackedTime;

    [SerializeField] float attackRange = 2.0f;



    void Awake()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        isTailing = false;
        isAttacking = false;
        followTimer = 0.0f;
        readyAttackTimer = 0.0f;
        attackCoolTimeTimer = 0.0f;
        attackedTimer = 0.0f;
    }


    void FixedUpdate()
    {
        AnimationManager();
        FollowPlayer();
        
        
    }

    

    public void StartTail()
    {
        isTailing = true;
        followTimer = followTime;
        enemySidePatrol.StopPatrol();
        emoteAnim.SetTrigger("alerted");

        anim.speed = 2;
    }

    void StopTailing() 
    {
        isTailing = false;
        enemySidePatrol.ResumePatrol();

        anim.speed = 1;
        
        
    }

    void FollowPlayer()
    {
        if (!isTailing)
        {
            return;
        }
        else
        {
            FollowTimerControl();

            if (!isTailing) 
            {
                StopTailing();
                return;
            }

            
            if (transform.position.x < playerPosition.x)
            {
                rb.linearVelocityX = followSpeed;
            }
            else if (transform.position.x > playerPosition.x)
            {
                rb.linearVelocityX = - followSpeed;
            }

            if (attackedTimer <= 0.0f)
            {
                Flip();
            }

        }
        
    }


    public void GetPlayerPosition( Vector2 playerPosition)
    {
        this.playerPosition = playerPosition;
    }

    void FollowTimerControl()
    {
        if (enemyVision.enemySeen)
        {
            followTimer = followTime;
        }
        else
        { 
            if (followTimer > 0f)
            {
                followTimer -= Time.fixedDeltaTime;
            }
            else
            {
                isTailing = false;
            }

        }
    }

    void Flip()
    {
        if (rb.linearVelocityX < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rb.linearVelocityX > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    void AnimationManager()
    {
        if (rb.linearVelocityX != 0f)
        {
            anim.SetBool("ismoving" , true);
        }

    }

    //bool knockBack;
    //public IEnumerator EnemyKnockedBack(bool enemyOnRight,int knockBackForceX , int knockBackForceY ,float  knockBackTime)
    //{
    //    knockBack = true;
    //    rb.linearVelocityY = knockBackForceY;
    //    if (enemyOnRight)
    //    {
    //        rb.linearVelocityX = knockBackForceX;
    //        transform.rotation = Quaternion.Euler(0, 180, 0);
    //    }
    //    else
    //    {
    //        rb.linearVelocityX = -knockBackForceX;
    //        transform.rotation = Quaternion.Euler(0, 0, 0);
    //    }
    //    yield return new WaitForSeconds(knockBackTime);
    //    knockBack = false;

    //}
}
