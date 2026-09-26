using System.Collections;
using UnityEngine;

public class SpikeandTrapTouchDamage : MonoBehaviour
{
    [SerializeField] Transform respawnTransform;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] int onTouchDamage = 1;
    [SerializeField] FadeToBlack fadeToBlack;

    //when we touch a spike were we in grace period?
    bool whenHitGrace;

    EnemyHealth enemyHealth;


    private void Awake()
    {
        whenHitGrace = false;
        onTouchDamage = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            whenHitGrace = playerHealth.gracePeriod;
            playerHealth.TakeHit(onTouchDamage);
            var playerTransform = collision.transform;
            StartCoroutine(SpikeTouchRoutine(playerTransform));

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TouchSpike();
        }
    }

    IEnumerator SpikeTouchRoutine(Transform transform)
    {
        StartCoroutine(playerController.GetKnockedBack(PlayerOnRight(transform)));
        if (playerHealth.currentHealth <= 0 || whenHitGrace) { yield break; }
        yield return StartCoroutine(fadeToBlack.FadetoBlack());
        playerController.playertransform.position = respawnTransform.position;
        StartCoroutine(fadeToBlack.FadeBackfromBlack());
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

}
