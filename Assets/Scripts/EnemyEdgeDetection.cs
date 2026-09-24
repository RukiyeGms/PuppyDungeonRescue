using UnityEngine;

public class EnemyEdgeDetection : MonoBehaviour
{

    [SerializeField] EnemySidePatrol enemySidePatrol;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Edge") && enemySidePatrol.isPatrolling && enemySidePatrol.comeBackTimer <=0)
        {
            enemySidePatrol.ChangeDirection();
        }
    }

}
