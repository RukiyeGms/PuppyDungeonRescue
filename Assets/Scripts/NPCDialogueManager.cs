using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] NPCDialogue[] dialogues;
    [SerializeField] NPC npc;
    //[SerializeField] NPC npc2;

    List<NPCDialogue> dialoguereadlist = new List<NPCDialogue>() { };
    PlayerHealth playerHealth;



    private void Awake()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }
    private void Start()
    {
        if (dialogues.Length == 0)
        {
            return;
        }
        dialoguereadlist.Clear();
        npc.ChangeDialogue(dialogues[0]);
    }

    public void AddReadList(NPCDialogue dialogue)
    {

        if (!dialoguereadlist.Contains(dialogue))
        {
            dialoguereadlist.Add(dialogue);
        }

    }

    public void CheckActiveDialogue()
    {
        /// element 0 first after element 0 is read element 1
        /// after element 1  read element 2 after read elemnt 3 and if no other options apply go back to readin element 1
        /// 
        //If Puppy needs heal and didn't heal yet heal dialouge element 4

        //if player has key element 5 (open the cage)
        //if element 5 is read element6 (pickup sheep)

        if (!dialoguereadlist.Contains(dialogues[0])) //if you didn't read the first dilogue yet
        {
            npc.ChangeDialogue(dialogues[0]);
        }
        else if (playerHealth.currentHealth < 5 && !dialoguereadlist.Contains(dialogues[4]))
        {
            npc.ChangeDialogue(dialogues[4]);
            //heal
        }
        else if (dialoguereadlist.Contains(dialogues[5])) //always free after key dialogue
        {
            npc.ChangeDialogue(dialogues[6]);
        }
        else if (GameController.AskKey())
        {
            npc.ChangeDialogue(dialogues[5]);
        }
        else
        {
            if (!dialoguereadlist.Contains(dialogues[1]))
            {
                npc.ChangeDialogue(dialogues[1]);
            }
            else if (!dialoguereadlist.Contains(dialogues[2]))
            {
                npc.ChangeDialogue(dialogues[2]);
            }
            else if (!dialoguereadlist.Contains(dialogues[3]))
            {
                npc.ChangeDialogue(dialogues[3]);
            }
            else
            {
                npc.ChangeDialogue(dialogues[1]);
            }


        }


    }

    public void SheepHeal()
    {
        playerHealth.GetHealth(3);
        SoundEffectManager.Play("Heal");
    }


}
