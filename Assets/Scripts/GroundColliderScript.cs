using UnityEngine;

public class GroundColliderScript : MonoBehaviour
{
    BoxCollider2D groundCollider;

    void Awake()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Wall") && collision.gameObject.layer != LayerMask.NameToLayer("Border"))
        {
            Physics2D.IgnoreCollision(collision.collider, groundCollider);
        }
    }
}
