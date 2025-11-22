using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dialogText;
    public string[] dialogue; 
    private int index = 0;
    public GameObject continueButton;
    public float wordSpeed = 0.03f;
    public bool playerInRange = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (dialogueBox.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                dialogueBox.SetActive(true);
                dialogText.text = "";
                StartCoroutine(Typing());
            }
        }

        if(dialogText.text == dialogue[index])
        {
            continueButton.SetActive(true);
        }
    }

    public void ZeroText()
    {
        dialogueBox.SetActive(false);
        dialogText.text = "";
        index = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private IEnumerator Typing()
    {
        if (dialogue == null || dialogue.Length == 0) yield break;

        string line = dialogue[index];
        dialogText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {

        continueButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            ZeroText();
        }
    }
}
