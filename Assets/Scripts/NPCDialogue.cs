using UnityEngine;


[CreateAssetMenu(fileName = "NewNPCDialouge", menuName = "NPC Dialouge")]
public class NPCDialogue : ScriptableObject
{
    public string[] npcName;
    public  Sprite npcArt;
    public  float writingSpeed = 0.05f;
    public  string[] dialougeLines;

    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

    public AudioClip voicesound;
    public float voicePitch = 1.0f;

    
}
