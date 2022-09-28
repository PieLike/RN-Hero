using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    public GameObject objInterface;
    public GameObject MainInterface, Voculabrary, UsingSpell, EnglishGame, Menu, DialogueSystem, YouGotMessage;

    private Queue<Message> messages;

    private void Start() 
    {
        objInterface = gameObject;







        messages = new Queue<Message>();
    }
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private class Message
    {

    }
}
