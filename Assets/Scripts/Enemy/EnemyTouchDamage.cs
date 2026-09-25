using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] int onTouchDamage = 1;

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
