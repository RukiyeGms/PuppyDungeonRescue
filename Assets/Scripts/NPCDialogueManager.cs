using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] NPCDialogue[] dialogues;
    NPC npc;

    

    private void Start()
    {
        npc = GetComponent<NPC>();
        if (dialogues.Length == 0)
        {
            return;
        }
        npc.ChangeDialogue(dialogues[0]);
    }

}
