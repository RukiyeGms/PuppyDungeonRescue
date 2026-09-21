using UnityEngine;

public class WallCheck : MonoBehaviour
{
    
    [SerializeField] PlayerController playerController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            
            playerController.wall = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            
            playerController.wall = false;
        }
    }
}
