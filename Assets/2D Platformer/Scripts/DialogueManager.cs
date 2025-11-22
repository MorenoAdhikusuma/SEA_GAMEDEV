using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.02f;

    [Header("References")]
    public Animator animator;
    public TMP_Text dialogueText;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    public bool isDialogueActive { get; private set; } = false;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogue == null)
            return;

        isDialogueActive = true;

        if (animator != null)
            animator.Play("DialogueBox_Open");

        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.dialogue)
        {
            dialogueQueue.Enqueue(line);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeText(line.text));
    }

    public void EndDialogue()
    {
        isDialogueActive = false;

        if (animator != null)
            animator.Play("DialogueBox_Close");

        StopAllCoroutines();

        if (dialogueText != null)
            dialogueText.text = string.Empty;
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
