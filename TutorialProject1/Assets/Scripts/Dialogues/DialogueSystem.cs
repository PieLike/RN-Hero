using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private string nameNpc;
    private GameObject dialoguePanel, dialogueName;//, dialogueText, 
    private GameObject messagePanel; private List<GameObject> messages = new List<GameObject>();
    private GameObject choicePanel;
    private Animator dialoguePanelAnimator;// private TMP_Text dialogueTextComponent;
    public enum emotions {neutral, happy, angry};
    private List<Sentence> sentences; public int sentenceNumber;
    private List<ChoiceButton> choiceButtons; private bool choiceWaiting = false, closeWaiting = false;

    private class ChoiceButton
    {
        public GameObject button; public TMP_Text text;  
    }

    private void Start() 
    {
        dialoguePanel = transform.Find("DialoguePanel2").gameObject;
            dialogueName = dialoguePanel.transform.Find("DialogueName").gameObject;   
            //dialogueText = dialoguePanel.transform.Find("DialogueText").gameObject; 
        //dialogueTextComponent = dialogueText.GetComponent<TMP_Text>(); 
        dialoguePanelAnimator = dialoguePanel.GetComponent<Animator>();

        messagePanel = Resources.Load<GameObject>("2d_prefabs/DMessagePanel");

        choicePanel = transform.Find("DialogueChoicePanel").gameObject;

        choiceButtons = new List<ChoiceButton>();
        for (int i = 0; i < 3; i++)
        {
            ChoiceButton choiceButton = new ChoiceButton();
            choiceButton.button = choicePanel.transform.Find($"DChoice{i+1}").gameObject;
            GameObject textObj = choiceButton.button.transform.Find("Text (TMP)").gameObject; 
            choiceButton.text = textObj.GetComponent<TMP_Text>();

            choiceButtons.Add(choiceButton);            
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialoguePanel.activeSelf == false)
            dialoguePanel.SetActive(true);

        dialoguePanelAnimator.SetBool("show", true);
        nameNpc = dialogue.name;       

        sentences = dialogue.sentences;
        sentenceNumber = FindFirstSentence();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        Debug.Log("sdfsdfs");
        if (closeWaiting)
        {
            Debug.Log("close");
            CloseDialogue();
            return;
        }
        if(sentenceNumber+1 > sentences.Count)
        {
            Debug.Log("end");
            EndDialogue();
            return;
        }

        dialogueName.GetComponent<TMP_Text>().text = nameNpc; //sentences[i].speaker
        //Debug.Log(sentences[sentenceNumber].emotion.ToString());
        StopAllCoroutines();
        //StartCoroutine(TypeSentence(sentences[sentenceNumber].text));
        CreateNewMessage(sentences[sentenceNumber].text);

        if(sentences[sentenceNumber].multiply == false)
        {
            //Debug.Log(sentences[sentenceNumber].choices.Count.ToString());
            if(sentences[sentenceNumber].choices.Count == 1)
            {
                sentenceNumber = FindSentenceNumberByGuid(sentences[sentenceNumber].choices[0].targetGuid);
            }
            else
            {
                EndDialogue();
                return;
            }
        }
        else
        {
            //Debug.Log("multiply");
            DisplayChoiceWindow();   
        }
    }

    private void DisplayChoiceWindow()
    {
        if (choicePanel.activeSelf == false)
            choicePanel.SetActive(true);

        for(int i = 0; i < sentences[sentenceNumber].choices.Count; i++)
        {
            if (choiceButtons[i].button.activeSelf == false)
                choiceButtons[i].button.SetActive(true);
            choiceButtons[i].text.text = sentences[sentenceNumber].choices[i].name;
        }
        choiceWaiting = true;
    }

    public void ChoicePick(int ChoiceNumber)
    {
        if(choiceWaiting == true)
        {
            //Debug.Log("sentence " + sentenceNumber.ToString());
            //Debug.Log(ChoiceNumber.ToString());
            sentenceNumber = FindSentenceNumberByGuid(sentences[sentenceNumber].choices[ChoiceNumber].targetGuid);
            choiceWaiting = false;
            DisplayNextSentence();

            foreach(var button in choiceButtons)
                if (button.button.activeSelf == true)
                    button.button.SetActive(false);    

            if (choicePanel.activeSelf == true)
                choicePanel.SetActive(false);
        }
    }

    public void ChoicePick1()
    {
        ChoicePick(0);
    }
    public void ChoicePick2()
    {
        ChoicePick(1);
    }
    public void ChoicePick3()
    {
        ChoicePick(2);
    }

    private void EndDialogue()
    {
        closeWaiting = true;
    }
    private void CloseDialogue()
    {        
        dialoguePanelAnimator.SetBool("show", false);
        closeWaiting = false;
        messages.Clear();
    }

    private int FindSentenceNumberByGuid(string guid)
    {
        for(int i = 0; i < sentences.Count; i++)
        {
            if(sentences[i].guid == guid)
                return i;
        }
        return 0;
    }

    private int FindFirstSentence()
    {
        for(int i = 0; i < sentences.Count; i++)
        {
            if(sentences[i].first == true)
                return i;
        }
        Debug.Log("не найдено первого нода");
        return 0;    
    }

    private GameObject CreateNewMessage(string messageText = "")
    {
       // Vector3 position = new Vector3(0f,0f,0f);
        GameObject newMessage = Instantiate(messagePanel); 
        newMessage.transform.SetParent(dialoguePanel.transform); 

        var rectTransform = newMessage.GetComponent<RectTransform>(); 
        rectTransform.localScale = Vector3.one; 
        rectTransform.localRotation = Quaternion.Euler(0,0,0); 
//        Debug.Log(rectTransform.rect.height.ToString());
        rectTransform.localPosition = new Vector3(0f,0f,0f);
        rectTransform.anchoredPosition = new Vector3(0f,30f,0f);
          
        //Debug.Log(rectTransform.rect.bottom.ToString());
        
        GameObject dialogueText = newMessage.transform.Find("DialogueText").gameObject; 
        TMP_Text dialogueTextComponent = dialogueText.GetComponent<TMP_Text>();
        dialogueTextComponent.text = messageText;

        dialogueTextComponent.ForceMeshUpdate();

        int linesCount = 0; float lineHeight = rectTransform.rect.height; float offset = 0f;
        while (dialogueTextComponent.textInfo.lineCount > linesCount)
        {
            linesCount++;
            offset = linesCount * lineHeight +dialogueTextComponent.margin.y*2;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offset);
        }

        messages.Add(newMessage);    
        MoveOldMessages(offset);
            
        return messages[messages.Count-1];
    }

    private void MoveOldMessages(float offset)
    {
        Transform transform;
        for(int i = 0; i < messages.Count-1; i++)
        {
            transform = messages[i].GetComponent<Transform>();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + offset, transform.localPosition.z);
        }
    }

    /*IEnumerator TypeSentence(string sentence)
    {
        float speed = 1f;
        dialogueTextComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTextComponent.text += letter;
            yield return new WaitForSeconds(Time.deltaTime*speed);    
        }   
    }*/

    public Dialogue ConvertGraphToDialogue(DialogueContainer dialogueGraph)
    {
        var dialogue = new Dialogue();
        dialogue.sentencesCount = dialogueGraph.DialogueNodeData.Count;

        dialogue.sentences = new List<Sentence>();
        for(int i = 0; i < dialogue.sentencesCount; i++)
        {               
            Sentence newSentence = new Sentence();
            newSentence.guid = dialogueGraph.DialogueNodeData[i].NodeGUID;
            newSentence.text = dialogueGraph.DialogueNodeData[i].DialogueText; 
            newSentence.emotion = dialogueGraph.DialogueNodeData[i].emotion;
            newSentence.first = dialogueGraph.DialogueNodeData[i].EntryPoint;

            newSentence.choices = dialogueGraph.DialogueNodeData[i].Choices;
            if (dialogueGraph.DialogueNodeData[i].Choices != null)
                newSentence.multiply = (dialogueGraph.DialogueNodeData[i].Choices.Count > 1)? true : false;

            dialogue.sentences.Add(newSentence); 
        }

        return dialogue;
    }

    [Serializable]
    public class Dialogue 
    {
        public string name;
        public int sentencesCount;
        [SerializeField] public List<Sentence> sentences;// public List<Sentence> sentences { get { if (Sentences != null) return Sentences; else  { Debug.Log("return"); return new List<Sentence>();  } } set{ Sentences = value;}}
    }

    [Serializable]
    public class Sentence
    {
        public string guid, speaker, text;
        public emotions emotion;
        public bool multiply = false, first; 
        [SerializeField] public List<DialogueNodeData.DialogueChoice> choices;// public List<DialogueNodeData.DialogueChoice> choices { get { if (Choices != null) return Choices; else  { return new List<DialogueNodeData.DialogueChoice>();  } } set{ Choices = value;}}
    }
}
