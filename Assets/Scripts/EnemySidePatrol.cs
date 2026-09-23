using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


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

    

    //Enemy Script you need to make a Patrol() in there //OR here and get t his there
    //[SerializeField] TeethEnemy teethEnemy;

    [SerializeField] float closeEnough = 0.5f;
    [SerializeField] float patrolSpeed = 4f;

    bool isPatrolling;

    private void Awake()
    {
        isPatrolling = true;
        currentPoint = pointB;
        Flip();
    }

 

    void FixedUpdate()
    {
        Patrol();
    }


    //See the paths and points

    private void OnDrawGizmos()
    {
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

    void PatrolContinueAfterAttcak()
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

        isPatrolling = true;
    }


    void CheckPointChange()
    {
        if (Vector2.Distance(currentPoint.transform.position, enemyTransform.position) < 0.5f)
        {
            if (currentPoint == pointA) { currentPoint = pointB; }
            else { currentPoint = pointA; }
            Flip();
        }
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
}
