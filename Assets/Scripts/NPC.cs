using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour , IInterractable
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueLine , npcName;
    [SerializeField] Image npcImage;
    [SerializeField] Image puppyImage;

    bool isTyping, isDialogueActive;
    int dialogueIndex;
    NPCDialogue dialogue;

    PlayerController playerController;

    void Start()
    {
        isDialogueActive = false;
            
    }

    public void ChangeDialogue(NPCDialogue dialogue)
    {
        this.dialogue = dialogue;
    }

    public  void Interract()
    {
        if (isDialogueActive)
        {
            ContinueDialogue();
        }
        else
        {
            if (dialogue != null) 
            {
                StartDialogue();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            playerController.ChangeInterractable(this);
            //add press a anim
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.InterractableNull();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
        npcImage.sprite = dialogue.npcArt;
        npcName.text = dialogue.npcName[dialogueIndex];
        
        dialoguePanel.SetActive(true);

        //
        Time.timeScale = 0;

        StartCoroutine(TypeLine());
    }

    void ContinueDialogue()
    {

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueLine.text = dialogue.dialougeLines[dialogueIndex];
            isTyping = false;
        }
        else
        {
            StopAllCoroutines();
            dialogueIndex++;
            if (dialogueIndex >= dialogue.dialougeLines.Length)
            {
                FinishDialouge();
                return;
            }
            if (dialogue.npcName[dialogueIndex] != null)
            {
                npcName.text = dialogue.npcName[dialogueIndex];
            }
            
            StartCoroutine(TypeLine());

        }
        
    }

    void FinishDialouge()
    {
        isDialogueActive = false;
        Time.timeScale = 1;
        dialoguePanel.SetActive(false);
        dialogueLine.text = null;
        npcImage.sprite = null;
        npcName.text = null;

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueLine.SetText(string.Empty);

        foreach (char c in dialogue.dialougeLines[dialogueIndex])
        {
            dialogueLine.text += c;
            yield return new WaitForSecondsRealtime(dialogue.writingSpeed);
        }

        isTyping = false;

        if (dialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogue.autoProgressDelay);
            ContinueDialogue();
        }
    }
}
