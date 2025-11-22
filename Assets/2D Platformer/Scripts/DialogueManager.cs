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

    private void Update()
    {
    if (!isDialogueActive) return;

    // Press E or Space to go to the next line
    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
    {
        DisplayNextDialogueLine();
    }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogue == null) return;

        isDialogueActive = true;
        dialogueGroup.alpha = 1f;  // <-- SHOW UI like your health bar

        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.dialogue)
            dialogueQueue.Enqueue(line);

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

        dialogueGroup.alpha = 0f; // <-- HIDE UI

        StopAllCoroutines();
        dialogueText.text = "";
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
