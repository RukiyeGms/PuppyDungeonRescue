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
    [SerializeField] NPCDialogueManager dialogueManager;

    PlayerController playerController;

    [SerializeField] Animator pressAAnim;

    //Spesific dialogues
    [SerializeField] NPCDialogue healDialogue;
    [SerializeField] NPCDialogue keyDialogue;
    [SerializeField] NPCDialogue freeDialogue;

    [SerializeField] GameObject cagesprite;
    [SerializeField] GameObject pressAPos2;
    [SerializeField] GameObject sheepOnMap;

    void Start()
    {
        isDialogueActive = false;
        SetPressAAnim(false);
            
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
            SetPressAAnim(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.InterractableNull();
            SetPressAAnim(false);
        }
    }

    void StartDialogue()
    {
        dialogueManager.CheckActiveDialogue();
        isDialogueActive = true;
        dialogueIndex = 0;
        npcImage.sprite = dialogue.npcArt;
        npcName.text = dialogue.npcName[dialogueIndex];
        
        dialoguePanel.SetActive(true);

        

            //
            Time.timeScale = 0;

        HighlightSpeakerImage();
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

            HighlightSpeakerImage();
            
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

        dialogueManager.AddReadList(dialogue);
        if (dialogue == healDialogue) { dialogueManager.SheepHeal(); }
        else if (dialogue == keyDialogue) { CageUnlock(); }
        else if (dialogue == freeDialogue) { TakeSheep(); }

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueLine.SetText(string.Empty);

        foreach (char c in dialogue.dialougeLines[dialogueIndex])
        {
            dialogueLine.text += c;
            SoundEffectManager.PlayVoice(dialogue.voicesound, dialogue.voicePitch);
            yield return new WaitForSecondsRealtime(dialogue.writingSpeed);
            
        }

        isTyping = false;

        if (dialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogue.autoProgressDelay);
            ContinueDialogue();
        }
    }

    void HighlightSpeakerImage()
    {
        if (npcName.text == "Sheep")
        {
            npcImage.color = Color.white;
            puppyImage.color = Color.gray;
        }
        else if (npcName.text == "Puppy")
        {
            npcImage.color = Color.gray;
            puppyImage.color = Color.white;
        }
    }

    void SetPressAAnim(bool boolean)
    {
        pressAAnim.gameObject.SetActive(boolean);
    }

    public void CageUnlock()
    {   
        pressAAnim.gameObject.transform.position = pressAPos2.transform.position;
        cagesprite.gameObject.SetActive(false);
        SoundEffectManager.Play("Unlock");
        GameController.UseKey();
    }

    public void TakeSheep()
    {
        SoundEffectManager.Play("Baa");
        sheepOnMap.SetActive(false);
        this.gameObject.SetActive(false);
        GameController.GetSheep();
    }




}
