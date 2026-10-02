using UnityEngine;

public class EnemyEdgeDetection : MonoBehaviour
{
    bool collidingWithEdge;
    [SerializeField] EnemySidePatrol enemySidePatrol;

    private void Awake()
    {
        collidingWithEdge = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Edge") && enemySidePatrol.isPatrolling && enemySidePatrol.comeBackTimer <=0)
        {
            enemySidePatrol.ChangeDirection();
        }
        collidingWithEdge = true;


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidingWithEdge = false;
    }

    public void CheckEdge()
    {
        if (collidingWithEdge) 
        {
            enemySidePatrol.ChangeDirection();
        }
    }

}
