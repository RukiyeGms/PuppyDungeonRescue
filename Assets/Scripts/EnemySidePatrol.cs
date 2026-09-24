using UnityEngine;

// 2 point patrol checking X 
public class EnemySidePatrol : MonoBehaviour
{   
    //patrol points
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    GameObject currentPoint;
    
    //patrolling object
    [SerializeField] Transform enemyTransform;
    [SerializeField] Rigidbody2D rb;

    
    [SerializeField] float closeEnough = 0.5f;
    [SerializeField] float patrolSpeed = 2f;

    [SerializeField] float comeBackTime = 5f;
    float comeBackTimer;
    

    public bool isPatrolling;

    private void Awake()
    {
        isPatrolling = true;
        currentPoint = pointB;
        comeBackTimer = 0;
        
        Flip();
    }

 

    void FixedUpdate()
    {
        Patrol();
        MovePlatformPosition();
        
    }


    //See the paths and points

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;

        Gizmos.DrawSphere(pointA.transform.position, closeEnough);
        Gizmos.DrawSphere(pointB.transform.position, closeEnough);
        Gizmos.DrawLine(pointA.transform.position , pointB.transform.position); 
    }

    void Patrol()
    {
        if (isPatrolling)
        {
            CheckPointChange();
            if (currentPoint.transform.position.x - enemyTransform.position.x > 0)
            {
                rb.linearVelocityX = patrolSpeed;

            }
            else if (currentPoint.transform.position.x - enemyTransform.position.x < 0)
            {
                rb.linearVelocityX = -patrolSpeed;
            }

        }

    }

    public void ResumePatrol()
    {
        if (Vector2.Distance(enemyTransform.position, pointA.transform.position) <= Vector2.Distance(enemyTransform.position , pointB.transform.position))
        {
            currentPoint = pointB;
        }
        else
        {
            currentPoint = pointA;
        }

        Flip();
        comeBackTimer = comeBackTime;
        isPatrolling = true;
    }

     public void StopPatrol()
    {
        isPatrolling = false;
        rb.linearVelocityX = 0f;
    }


    void CheckPointChange()
    {
        if (Vector2.Distance(currentPoint.transform.position, enemyTransform.position) < closeEnough)
        {
            ChangeDirection();
        }
    }

    public void ChangeDirection()
    {
        if (currentPoint == pointA) { currentPoint = pointB; }
        else { currentPoint = pointA; }
        Flip();
    }

    void Flip()
    {
        if (currentPoint.transform.position.x - enemyTransform.position.x > 0)
        {
            enemyTransform.rotation = Quaternion.Euler(0, 180, 0);
            
        }
        else if (currentPoint.transform.position.x - enemyTransform.position.x  < 0)
        {
            enemyTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    //ifenemycan't reach old patrol after resuming patrol change patrol position
    void MovePlatformPosition()
    {
        if (comeBackTimer <= 0)
        {
            return;
        }
        else
        {
            
            if ((Vector2.Distance(enemyTransform.position, pointA.transform.position ) < closeEnough || Vector2.Distance(enemyTransform.position, pointB.transform.position) < closeEnough)) 
            { 
                comeBackTimer = 0;
                return;
            }
            else
            {
                comeBackTimer -= Time.fixedDeltaTime;
                if (comeBackTimer <= 0)
                {
                    transform.position = enemyTransform.position;

                }
            }
        }
    }
}
