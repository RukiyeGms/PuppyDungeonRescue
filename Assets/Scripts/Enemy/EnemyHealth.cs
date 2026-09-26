using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator teethAnim;
    TeethEnemy teethEnemy;
    SpriteRenderer spriteRenderer;

    int enemyMaxHealth = 3;
    int enemyHealth;
    public bool dead;

    private void Awake()
    {
        dead = false;
        teethAnim = GetComponent<Animator>();
        teethEnemy = GetComponent<TeethEnemy>();
        enemyHealth = enemyMaxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeHit(int damage)
    {
        if (!dead && enemyHealth > 0)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0) 
            { 
                Die();
                return;
            }
            else
            {
                StartCoroutine(FlashRed());
            }


        }
    }

    public void TouchSpike()
    {
        if (!dead)
        {
            Die();

        }
        
    }

    public void Die()
    {
        
        dead = true;
        teethAnim.speed = 1;
        teethEnemy.OnDeath();
        teethAnim.SetTrigger("dead");
        
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator FlashRed()
    {
        var tempcolor = spriteRenderer.color;
        tempcolor.b = 0;
        tempcolor.g = 0;
        spriteRenderer.color = tempcolor;
        yield return new WaitForSeconds(0.2f);
        tempcolor.b = 1;
        tempcolor.g = 1;
        spriteRenderer.color = tempcolor;
        
    }

}
