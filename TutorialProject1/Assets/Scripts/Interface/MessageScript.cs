using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{
    public enum Scale {TextBig, TextMedium, TextSmall};
    private Queue<Message> messages;
    private InterfaceManager interfaceManager;
    private Animator animatorYG, animatorOK; AnimationClip hideAnim;
    public class Message
    {
        public string label, text;
        public Scale scale = Scale.TextMedium;
    }

    private void Awake() 
    {
        messages = new Queue<Message>();
    }
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();        

        animatorYG = interfaceManager.YGPanel.GetComponent<Animator>();
            hideAnim = FindAnimation(animatorYG, "Hide");
        animatorOK = interfaceManager.OkMessagePanel.GetComponent<Animator>();
    }

    public void NewMessage(Message newMes)
    {
        messages.Enqueue(newMes);
    }    
    public void NewMessage(string label, string text, Scale scale = Scale.TextMedium)
    {
        Message newMes = new Message();
        newMes.label = label;
        newMes.text = text;
        newMes.scale = scale;
        NewMessage(newMes);
    }

    private void Update() 
    {
        if (messages.Count != 0 && interfaceManager.YGPanel.activeSelf == false && MainVariables.inInterface == false)
        {
            var mes = messages.Dequeue();
            ShowMessage(mes);
        }
    }
    private void ShowMessage(Message newMes)
    {
        interfaceManager.YGPanel.SetActive(true);
        animatorYG.SetBool("Show", true);

        interfaceManager.YGName.GetComponent<TMP_Text>().text = newMes.label;
        var text = interfaceManager.YGText.GetComponent<TMP_Text>();
            text.text = newMes.text;
            text.fontSize = FontSize(newMes.scale);

        StartCoroutine(HideMessageCoroutine(2f));
    }

    private IEnumerator HideMessageCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideMessage();  
    }
    private void HideMessage()
    {
        animatorYG.SetBool("Show", false);
        StartCoroutine(PlayHideAnimationCoroutine());
    }
    private IEnumerator PlayHideAnimationCoroutine()
    {        
        yield return new WaitForSeconds(hideAnim.length);
            interfaceManager.YGPanel.SetActive(false); 
    }
    public AnimationClip FindAnimation (Animator _animator, string name) 
    {
        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
    
    private float FontSize(Scale scale)
    {
        switch (scale)
        {
            case Scale.TextBig: return 40;
            case Scale.TextMedium: return 20;
            case Scale.TextSmall: return 12;
            default: return 20;
        }        
    }

    public void ShowOkMessage(Action buttonAction, string label = "")
    {
        OnGameStartAndUpdate.CloseInterface();
        interfaceManager.OkMessagePanel.SetActive(true);
        //MainVariables.inInterface = true;
        animatorOK.SetBool("Show", true);

        if (label != "") interfaceManager.OkLabel.GetComponent<TMP_Text>().text = label;    

        Button okButton = interfaceManager.OkButton.GetComponent<Button>();   
        okButton.onClick.AddListener(() => buttonAction());  
        okButton.onClick.AddListener(() => HideOkMessage()); 

        OnGameStartAndUpdate.OnInterfaceClose += HideOkMessage;
    }

    public void HideOkMessage()
    {
        interfaceManager.OkMessagePanel.SetActive(false);
        //MainVariables.inInterface = false;
        animatorOK.SetBool("Hide", true);

        OnGameStartAndUpdate.OnInterfaceClose -= HideOkMessage;
    }
}
