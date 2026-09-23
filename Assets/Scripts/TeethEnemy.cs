using UnityEngine;

public class TeethEnemy : MonoBehaviour
{

    Animator anim;
    bool isOnTail;
    bool isAttacking;
    Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationManager();
        
    }

    void AnimationManager()
    {
        if (rb.linearVelocityX != 0f)
        {
            anim.SetBool("ismoving" , true);
        }
    }
}
