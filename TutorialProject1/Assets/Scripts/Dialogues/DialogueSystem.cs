using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private Queue<string> sentences; private string nameNpc;
    private GameObject dialoguePanel, dialogueName, dialogueText;
    private Animator dialoguePanelAnimator; private TMP_Text dialogueTextComponent;

    private void Start() 
    {
        dialoguePanel = transform.Find("DialoguePanel").gameObject;
        dialoguePanel.SetActive(true);
        dialogueName = dialoguePanel.transform.Find("DialogueName").gameObject;   
        dialogueText = dialoguePanel.transform.Find("DialogueText").gameObject; 
        dialogueTextComponent = dialogueText.GetComponent<TMP_Text>(); 
        dialoguePanelAnimator = dialoguePanel.GetComponent<Animator>();

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanelAnimator.SetBool("show", true);
        nameNpc = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentencs)
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

        dialogueName.GetComponent<TMP_Text>().text = nameNpc;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    private void EndDialogue()
    {
        dialoguePanelAnimator.SetBool("show", false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        float speed = 1f;
        dialogueTextComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTextComponent.text += letter;
            yield return new WaitForSeconds(Time.deltaTime*speed);    
        }   
    }

    public class Dialogue 
    {
        public string name;
        public string[] sentencs;
    }
}
