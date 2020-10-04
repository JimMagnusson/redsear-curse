using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject witch;
    private string npcName;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        npcName = dialogue.name;
        Debug.Log("Starting dialogue with " + npcName);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        Debug.Log("End of conversation");
        WitchMovement witchMovement = witch.GetComponent<WitchMovement>();
        if (npcName == "The Witch")
        {
            if(!witchMovement.IsMoving() && !witchMovement.IsAtTarget())
            {
                witchMovement.SetIsMoving(true);
            }
            witch.GetComponent<NPC>().dialogue.sentences = new string[] { "Oh my, you survived this long!", "There, now your curse is removed!" };
            if (witchMovement.IsAtTarget())
            {
                FindObjectOfType<LoadHandler>().LoadNextScene();
            }
        }
    }
}
