using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //puppy health
    int maxHealthCap = 10;
    int maxHealth = 5;
    int currentHealth;

    [SerializeField] float gracePeriodTime = 1f;
    bool gracePeriod;
    float timer;
    float flickerRate = 0.1f;

    [SerializeField] HealthUI healthUI;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    

    private void Awake()
    {
        currentHealth = maxHealth;
        gracePeriod = false;

    }
    void Start()
    {
        healthUI.SetMaxHearts(maxHealth);
        
    }


    public void TakeHit(int damage)
    {
        if (currentHealth > 0 && !gracePeriod) 
        {
            currentHealth -= damage;
            healthUI.UpdateHearts(currentHealth);
            StartCoroutine(GracePeriod());

        }

        if (currentHealth <= 0) {/*Die*/}
    }

    public void GetHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthUI.UpdateHearts(currentHealth);
    }
    public void UpgradeMaxHealth(int upgrade)
    {
        maxHealth += upgrade;
        if (maxHealth > maxHealthCap) { maxHealth = maxHealthCap; }
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
    }

    IEnumerator GracePeriod()
    {
        gracePeriod = true;
        timer = 0f;
        while (timer < gracePeriodTime)
        {   
            playerSpriteRenderer.enabled = !playerSpriteRenderer.enabled;
            yield return new WaitForSeconds(flickerRate);
            timer += flickerRate;
        }
        gracePeriod = false;
        playerSpriteRenderer.enabled = true;

    }
}
