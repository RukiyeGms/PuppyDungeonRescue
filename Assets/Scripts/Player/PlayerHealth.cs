using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    

    //puppy health
    int maxHealthCap = 10;
    int maxHealth = 5;
    public int currentHealth;

    [SerializeField] float gracePeriodTime = 1f;
    public bool gracePeriod;
    float timer;
    float flickerRate = 0.1f;

    [SerializeField] HealthUI healthUI;
    [SerializeField] SpriteRenderer playerSpriteRenderer;

    //!For player playerController to subscribe
    public static event Action OnPuppyDied;

    CinemachineImpulseSource impulseSource;
    

    private void Awake()
    {
        currentHealth = maxHealth;
        gracePeriod = false;
        impulseSource = GetComponent<CinemachineImpulseSource>();

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
            CameraShakeManager.instance.CameraShake(impulseSource);

        }

        if (currentHealth <= 0) 
        {
            /*Die*/
            OnPuppyDied.Invoke();
        }
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
