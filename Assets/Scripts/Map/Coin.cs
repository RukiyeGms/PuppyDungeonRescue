using UnityEngine;

public class Coin : MonoBehaviour
{
    int amount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.CoinUp(amount);
            Destroy(gameObject);
        }
        
    }
}
