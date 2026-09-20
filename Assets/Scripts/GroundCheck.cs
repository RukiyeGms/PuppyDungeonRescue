using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }
}
