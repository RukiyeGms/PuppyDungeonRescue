using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Doors : MonoBehaviour , IInterractable  
{
    Transform playerTransform;
    PlayerController playerController;
    [SerializeField] Transform teleportTransform;
    [SerializeField] BoxCollider2D boxCollider;
    CinemachineConfiner2D confiner;

    void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.ChangeInterractable(this);
            playerTransform = collision.gameObject.GetComponent<Transform>();

        }
    }
    
    public void Interract()
    {
        playerTransform.position = teleportTransform.position;
        playerController.respawnTransform = teleportTransform;
        confiner.BoundingShape2D = boxCollider;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.InterractableNull();
        }
    }

   
}
