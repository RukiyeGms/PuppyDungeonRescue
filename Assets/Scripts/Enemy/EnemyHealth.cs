using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator teethAnim;
    TeethEnemy teethEnemy;

    int enemyMaxHealth = 3;
    int enemyHealth;
    public bool dead;

    private void Awake()
    {
        dead = false;
        teethAnim = GetComponent<Animator>();
        teethEnemy = GetComponent<TeethEnemy>();
        enemyHealth = enemyMaxHealth;
    }

    public void TakeHit(int damage, bool playerOnRight, float knockBackForce)
    {
        if (!dead && enemyHealth > 0)
        {
            enemyHealth -= damage;
            teethAnim.SetTrigger("hurt");
            SoundEffectManager.Play("MonsterRoar");
            StartCoroutine(teethEnemy.EnemyKnockedBack(playerOnRight, knockBackForce , enemyHealth));
            

            if (enemyHealth <= 0) 
            { 
                Die();
                return;
            }
            else
            {
                //hurtanimation
                
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

 

}
