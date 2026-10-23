using System.Collections;
using TMPro;
using UnityEngine;

public class Enterance : MonoBehaviour, IInterractable
{
    Animator pressAAnim;
    SpriteRenderer pressASprite;
    PlayerController playerController;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject npcImage;
    [SerializeField] TextMeshProUGUI puppyName;
    [SerializeField] TextMeshProUGUI dialogueText;

    [SerializeField] string cannotLeaveDialogue = "I need to rescue the adventurer.";
    [SerializeField] string pupname = "Puppy";

    bool dialogueOn;
    bool isTyping;

    private void Awake()
    {
        pressAAnim = GetComponentInChildren<Animator>();
        pressASprite = pressAAnim.gameObject.GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        pressAAnim.enabled = false;
        pressASprite.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressAAnim.enabled = true;
            pressASprite.enabled=true;
            playerController = collision.GetComponent<PlayerController>();
            playerController.ChangeInterractable(this);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressAAnim.enabled = false;
            pressASprite.enabled = false;
            playerController.InterractableNull();

        }
    }

    public void Interract()
    {
        if (GameController.AskSheep())
        {
            //finish game
            return;
        }

        if (!dialogueOn)
        {
            dialogueOn = true;
            TurnOnPanel();
            StartCoroutine(TypeLine());
        }
        else
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = cannotLeaveDialogue;
                isTyping = false;
            }
            else
            {
                TurnOffPanel();
                dialogueOn = false;
            }
        }

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText(string.Empty);

        foreach (char c in cannotLeaveDialogue)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        isTyping = false;
    }

    void TurnOnPanel()
    {
        dialoguePanel.SetActive(true);
        npcImage.SetActive(false);
        puppyName.text = pupname;
    }

    void TurnOffPanel()
    {
        npcImage.SetActive(true);
        puppyName.text = "";
        dialoguePanel.SetActive(false);
        
    }

}
