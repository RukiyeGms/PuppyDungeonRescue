using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    public bool enemySeen;

    //Get enemy Script
    [SerializeField] TeethEnemy teethEnemy;

    float playerPositionKnownTime;
    float playerPositionKnownTimer;
    bool visionTimerON;

    GameObject player;


    void Awake()
    {
        enemySeen = false;
        playerPositionKnownTime = teethEnemy.followTime;
        playerPositionKnownTimer = 0;
        
    }

    private void FixedUpdate()
    {
        VisionTimer();
        GetPlayerPosition();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.CompareTag("Player"))
        {
            enemySeen = true;
            teethEnemy.StartTail();
            playerPositionKnownTimer = playerPositionKnownTime;
            player = collision.gameObject;
        }
    }

 


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemySeen = false;
        }
    }

    void GetPlayerPosition()
    {
        if (enemySeen || visionTimerON)
        {
            teethEnemy.GetPlayerPosition(player.transform.position);
        }

    }

    //return true if not ended
    void VisionTimer()
    {
        if (playerPositionKnownTimer <= 0)
        {
            visionTimerON =  false;
        }
        else
        {
            if (enemySeen) { playerPositionKnownTimer = playerPositionKnownTime; }
            playerPositionKnownTimer -= Time.fixedDeltaTime;
            visionTimerON = true; 
        }
    }

    
}
