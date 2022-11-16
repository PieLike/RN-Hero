using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private string nameNpc;
    private GameObject dialoguePanel, dialogueName;//, dialogueText, 
    private GameObject messagePanelRight, messagePanelLeft; private List<GameObject> messages = new List<GameObject>();
    private GameObject choicePanel;
    private Animator dialoguePanelAnimator;// private TMP_Text dialogueTextComponent;
    public enum emotions {neutral, happy, angry};
    private List<Sentence> sentences; public int sentenceNumber;
    private List<ChoiceButton> choiceButtons; private bool choiceWaiting = false, closeWaiting = false;
    private InterfaceManager interfaceManager;

    private class ChoiceButton
    {
        public GameObject button; public TMP_Text text;  
    }

    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();

        dialoguePanel = interfaceManager.DialoguePanel2; 
        dialogueName =  interfaceManager.DialogueName;  
        
        dialoguePanelAnimator = dialoguePanel.GetComponent<Animator>();

        messagePanelRight = Resources.Load<GameObject>("2d_prefabs/DMessagePanelRight");
        messagePanelLeft = Resources.Load<GameObject>("2d_prefabs/DMessagePanelLeft");

        choicePanel = interfaceManager.DialogueChoicePanel;

        choiceButtons = new List<ChoiceButton>();
        for (int i = 0; i < 3; i++)
        {
            ChoiceButton choiceButton = new ChoiceButton();
            //choiceButton.button = choicePanel.transform.Find($"DChoice{i+1}").gameObject;
            choiceButton.button = interfaceManager.DialogueChoicePanel.transform.Find($"DChoice{i+1}").gameObject;
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
        nameNpc = dialogue.speaker; 
        dialogueName.GetComponent<TMP_Text>().text = nameNpc;      

        sentences = dialogue.sentences;
        sentenceNumber = FindFirstSentence();

        MainVariables.inTalking = true;        

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (closeWaiting)
        {
            //Debug.Log("close");
            CloseDialogue();
            return;
        }
        if(sentenceNumber+1 > sentences.Count)
        {
            //Debug.Log("end");
            EndDialogue();
            return;
        }

        //Debug.Log(sentences[sentenceNumber].emotion.ToString());
        StopAllCoroutines();
        //StartCoroutine(TypeSentence(sentences[sentenceNumber].text));
        CreateNewMessage(sentences[sentenceNumber].text, sentences[sentenceNumber].heroLine);

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

        foreach(var mes in messages)
        {
            Destroy(mes);
        }
        messages.Clear();

        MainVariables.inTalking = false;
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

    private GameObject CreateNewMessage(string messageText = "", bool right = true)
    {
        GameObject newMessage;
        if (right)
            newMessage = Instantiate(messagePanelRight, dialoguePanel.transform); 
        else
            newMessage = Instantiate(messagePanelLeft, dialoguePanel.transform); 
              
        RectTransform newMessageTale  = newMessage.transform.Find("DialogueTale").gameObject.GetComponent<RectTransform>();
        //newMessage.transform.SetParent(dialoguePanel.transform); 

        var rectTransform = newMessage.GetComponent<RectTransform>(); 
        rectTransform.localScale = Vector3.one; 

        if (right)
            rectTransform.localRotation = Quaternion.Euler(0,0,0);
        else
            rectTransform.localRotation = Quaternion.Euler(0,180f,0); 

        rectTransform.localPosition = new Vector3(0f,0f,0f);
        rectTransform.anchoredPosition = new Vector3(0f,30f + newMessageTale.rect.height,0f);
        
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
        MoveOldMessages(offset + newMessageTale.rect.height);
            
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
        dialogue.speaker = dialogueGraph.NpcName;

        dialogue.sentences = new List<Sentence>();
        for(int i = 0; i < dialogue.sentencesCount; i++)
        {               
            Sentence newSentence = new Sentence();
            newSentence.guid = dialogueGraph.DialogueNodeData[i].NodeGUID;
            newSentence.text = dialogueGraph.DialogueNodeData[i].DialogueText; 
            newSentence.emotion = dialogueGraph.DialogueNodeData[i].emotion;
            newSentence.first = dialogueGraph.DialogueNodeData[i].EntryPoint;
            newSentence.heroLine = dialogueGraph.DialogueNodeData[i].HeroLine;

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
        public string name, speaker;
        public int sentencesCount;
        [SerializeField] public List<Sentence> sentences;// public List<Sentence> sentences { get { if (Sentences != null) return Sentences; else  { Debug.Log("return"); return new List<Sentence>();  } } set{ Sentences = value;}}
    }

    [Serializable]
    public class Sentence
    {
        public string guid, text;
        public emotions emotion;
        public bool multiply = false, first, heroLine; 
        [SerializeField] public List<DialogueNodeData.DialogueChoice> choices;// public List<DialogueNodeData.DialogueChoice> choices { get { if (Choices != null) return Choices; else  { return new List<DialogueNodeData.DialogueChoice>();  } } set{ Choices = value;}}
    }
}
