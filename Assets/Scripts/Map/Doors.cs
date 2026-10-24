using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Doors : MonoBehaviour , IInterractable  
{
    Transform playerTransform;
    PlayerController playerController;
    [SerializeField] Transform teleportTransform;
    [SerializeField] BoxCollider2D boxCollider;
    CinemachineConfiner2D confiner;
    SpriteRenderer playerSprite;

    FadeToBlack fadeToBlack;
    Animator doorInstructionAnimator;
    [SerializeField] SpriteRenderer doorInstructionSprite;

    [SerializeField] Image nextRoom;

    void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        fadeToBlack = FindAnyObjectByType<FadeToBlack>();
        doorInstructionAnimator = GetComponentInChildren<Animator>();
        SetDoorInstructionAnim(false);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.ChangeInterractable(this);
            playerTransform = collision.gameObject.GetComponent<Transform>();
            playerSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            SetDoorInstructionAnim(true);


        }
    }

    public void Interract()
    {
        
        StartCoroutine(FadeinandOut());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.InterractableNull();
            SetDoorInstructionAnim(false);
        }
    }

    IEnumerator FadeinandOut()
    {
        playerSprite.enabled = false;
        playerController.PlayeCanMove(false);
        yield return StartCoroutine(fadeToBlack.FadetoBlack());
        playerTransform.position = teleportTransform.position;
        playerController.respawnTransform = teleportTransform;
        confiner.BoundingShape2D = boxCollider;
        GameController.ChangeMap(nextRoom);
        
        playerSprite.enabled = true;
        yield return new WaitForSeconds(1f);
        playerController.PlayeCanMove(true);
        yield return StartCoroutine (fadeToBlack.FadeBackfromBlack());
    }

    void SetDoorInstructionAnim(bool boolean)
    {
        doorInstructionAnimator.enabled = boolean;
        doorInstructionSprite.enabled = boolean;
    }

   
}
