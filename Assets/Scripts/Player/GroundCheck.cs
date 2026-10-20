using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] PlayerController playerController;

    //hold the coroutine to stop if necessary
    Coroutine coyoteCoroutine;
    int groundedobjects;

    private void Awake()
    {
        coyoteCoroutine = null;
        groundedobjects = 0;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (coyoteCoroutine != null)
            {
                StopCoroutine(coyoteCoroutine);
                coyoteCoroutine = null;
            }

            playerController.SetGrounded(true);
            groundedobjects++;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (gameObject.activeInHierarchy) 
            {
                groundedobjects--;

                if (groundedobjects == 0) { coyoteCoroutine = StartCoroutine(CoyoteTime()); }
                
            } 
        }
    }

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(0.1f);

        playerController.SetGrounded(false);
        coyoteCoroutine = null;
    }
}
