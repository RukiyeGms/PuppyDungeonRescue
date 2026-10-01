using System.Collections;
using UnityEngine;

public class SpikeandTrapTouchDamage : MonoBehaviour
{
    PlayerController playerController;
    PlayerHealth playerHealth;
    [SerializeField] int onTouchDamage = 1;
    FadeToBlack fadeToBlack;

    //when we touch a spike were we in grace period?
    bool whenHitGrace;

    EnemyHealth enemyHealth;


    private void Awake()
    {
        whenHitGrace = false;
        onTouchDamage = 1;
        fadeToBlack = FindAnyObjectByType<FadeToBlack>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        playerController = FindAnyObjectByType<PlayerController>();
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
        playerController.playertransform.position = playerController.respawnTransform.position;
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
