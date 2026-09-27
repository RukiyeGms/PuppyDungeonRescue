using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    int attackPower = 1;
    Animator anim;
    Coroutine attackRoutine;
    EnemyHealth enemyHealth;
    PlayerController playerController;
    [SerializeField] Transform playerTransform;
    Rigidbody2D rb;
    [SerializeField] float attackDashSpeed = 4;

    Transform enemyTransform;

    float knockbackForceX = 10;

    CircleCollider2D circleCollider2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        anim = GetComponentInParent<Animator>();
        enemyHealth = null;
        playerController = GetComponentInParent<PlayerController>();
        rb = GetComponentInParent<Rigidbody2D>();
        enemyTransform = null;
        
    }

    private void Start()
    {
        circleCollider2D.enabled = false;
        attackRoutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //now enemyhealth
            enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyTransform = enemyHealth.transform;
            enemyHealth.TakeHit(attackPower , PlayeronRight(playerTransform , enemyTransform) , knockbackForceX);
        }
    }

    public void ChangeAttackPower(int newPower)
    {
        attackPower = newPower;
    }

    void OnAttack(InputValue value)
    {
        if (attackRoutine != null) { return; }
        attackRoutine = StartCoroutine(AttackRoutine());

    }

    IEnumerator AttackRoutine()
    {
        if (FacingRight()) { rb.linearVelocityX = attackDashSpeed; }
        else { rb.linearVelocityX = -attackDashSpeed; }
        
        playerController.isAttacking = true;
        anim.SetBool("isAttacking", true);
        circleCollider2D.enabled = true;
        yield return new WaitForSeconds(0.2f);
        circleCollider2D.enabled = false;
        anim.SetBool("isAttacking", false);
        playerController.isAttacking = false;
        rb.linearVelocityX =0;

        //wait for attacking again
        yield return new WaitForSeconds(0.2f);
        attackRoutine = null;
    }

    bool FacingRight()
    {

        if (playerTransform.rotation.y == 0)
        {
            return true;
        }
        else { return false; }
    }

    bool PlayeronRight(Transform player , Transform enemy)
    {
        if (player.position.x > enemy.position.x)
        {
            return true;
        }
        else { return false; }
    }
}
