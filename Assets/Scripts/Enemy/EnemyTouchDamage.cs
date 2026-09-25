using System.Collections;
using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] TeethEnemy teethEnemy;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] int onTouchDamage = 1;

    float turnToPlayerTime = 0.1f;

    private void Awake()
    {
        onTouchDamage = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //player controlle knockedback
            playerHealth.TakeHit(onTouchDamage);
            StartCoroutine(playerController.GetKnockedBack(PlayerOnRight(collision.gameObject.transform)));
            StartCoroutine(WaitOneFrameToTail());


        }
    }
    bool PlayerOnRight(Transform playerTransform)
    {
        if (transform.position.x >= playerTransform.position.x)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    IEnumerator WaitOneFrameToTail(){

        yield return new WaitForSeconds(turnToPlayerTime);
        teethEnemy.StartTail();
    }
}
