using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
  


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerController.SetGrounded(true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            playerController.SetGrounded(false);
        }
    }
}
