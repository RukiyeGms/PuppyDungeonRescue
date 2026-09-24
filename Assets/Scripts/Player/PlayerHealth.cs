using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //puppy health
    int maxHealthCap = 10;
    int maxHealth = 5;
    int currentHealth;

    [SerializeField] float gracePeriodTime = 0.5f;
    float gracePeriodTimer;

    [SerializeField] HealthUI healthUI;
    

    private void Awake()
    {
        currentHealth = maxHealth;
        gracePeriodTimer = 0;

    }
    void Start()
    {
        healthUI.SetMaxHearts(maxHealth);
        
    }

    private void FixedUpdate()
    {
        GracePeriod();
    }

    public void TakeHit(int damage)
    {
        if (currentHealth > 0 && gracePeriodTimer <= 0)
        {
            currentHealth -= damage;
            healthUI.UpdateHearts(currentHealth);
            gracePeriodTimer = gracePeriodTime;

        }

        if (currentHealth < 0) {/*Die*/}
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

    void GracePeriod()
    {
        if (gracePeriodTimer > 0)
        {
            gracePeriodTimer -= Time.fixedDeltaTime;
        }

    }
}
