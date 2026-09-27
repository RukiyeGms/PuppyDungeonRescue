using System.Collections;
using UnityEngine;

public class TeethEnemy : MonoBehaviour
{
    Vector2 playerPosition;
    Animator anim;
    [SerializeField] Animator emoteAnim;
    bool isTailing;
    bool knockBack;


    [SerializeField] EnemyVision enemyVision;

    //disable on dead
    [SerializeField] GameObject enemyVisionObject;
    [SerializeField] GameObject enemyTouchOBject;
    [SerializeField] GameObject enemyEmoteObject;
    [SerializeField] GameObject enemyEdgeDetectObject;

    [SerializeField] EnemySidePatrol enemySidePatrol;
    Rigidbody2D rb;

    [SerializeField] float followSpeed = 6f;
    public float followTime = 1.0f;
    float followTimer;

    bool dead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        isTailing = false;
        followTimer = 0.0f;
        dead = false;
        knockBack = false;
        
        
    }


    void FixedUpdate()
    {
        if (!dead && !knockBack)
        {
            AnimationManager();
            FollowPlayer();
        }        
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

            Flip();

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

    public void OnDeath()
    {
        dead = true;
        enemySidePatrol.Dead();
        rb.linearVelocityX = 0;
        Destroy(enemyEdgeDetectObject);
        Destroy(enemyEmoteObject);
        Destroy(enemyTouchOBject);
        Destroy(enemyVisionObject);
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

    public IEnumerator EnemyKnockedBack(bool playerOnRight, float knockBackForceX , int currenthealth)
    {
        enemySidePatrol.StopPatrol();
        knockBack = true;
        if (playerOnRight)
        {
            rb.linearVelocityX = -knockBackForceX;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rb.linearVelocityX = knockBackForceX;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        yield return new WaitForSeconds(0.2f);
        knockBack = false;
        if (currenthealth > 0) { StartTail(); }
        

    }
}
