using UnityEngine;

public class SpikeandTrapTouchDamage : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] int onTouchDamage = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        onTouchDamage = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeHit(onTouchDamage);
            StartCoroutine(controller.GetKnockedBack(PlayerOnRight(collision.gameObject.transform)));

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

}
