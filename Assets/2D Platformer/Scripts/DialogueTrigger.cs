using System.Collections.Generic;
using UnityEngine;

public enum DialogueCharacter
{
    NPC,
    Player,
    System
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;

    [TextArea(3, 10)]
    public string text;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogue = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Data")]
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
}
