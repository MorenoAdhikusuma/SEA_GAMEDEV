using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.02f;

    [Header("UI")]
    public TMP_Text dialogueText;
    public CanvasGroup dialogueGroup;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    public bool isDialogueActive { get; private set; } = false;

    private bool isTyping = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        dialogueGroup.alpha = 0f;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogue == null) return;

        isDialogueActive = true;
        dialogueGroup.alpha = 1f;

        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.dialogue)
            dialogueQueue.Enqueue(line);

        DisplayNextDialogueLine();
    }

    // 🔹 FUNCTION YANG DIPANGGIL BUTTON
    public void OnNextDialogue()
    {
        if (!isDialogueActive) return;

        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
            return;
        }

        DisplayNextDialogueLine();
    }

    private void DisplayNextDialogueLine()
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
        dialogueGroup.alpha = 0f;

        StopAllCoroutines();
        dialogueText.text = "";
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
