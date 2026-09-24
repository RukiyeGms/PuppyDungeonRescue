using System.Collections.Generic;
using UnityEngine;

public class GroundColliderScript : MonoBehaviour
{
    List<string> doCollide = new List<string>() { "Ground", "Wall", "Border" };
    BoxCollider2D groundCollider;

    void Awake()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLayerinList(collision.gameObject))
        {
            Physics2D.IgnoreCollision(collision.collider, groundCollider);
        }
    }

    bool isLayerinList(GameObject collided)
    {
        foreach (string layername in doCollide )
        {
            if (collided.layer == LayerMask.NameToLayer(layername) )
            {
                return true;
            }
        }
        return false;
       
    }
}
