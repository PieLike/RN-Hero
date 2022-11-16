using UnityEngine;
using TMPro;
using System;

public class WavesCounter : MonoBehaviour
{
    static InterfaceManager interfaceManager;
    static TMP_Text waves;

    void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        waves = interfaceManager.Waves.GetComponent<TMP_Text>();
    }
    
    public static void Show()
    {
        interfaceManager.Waves.SetActive(true);
    }
    public static void Hide()
    {
        interfaceManager.Waves.SetActive(false);
    }
    public static void Clear()
    {
        waves.text = "Отбито волн: 0";
    }
    public static void AddCount()
    {
        if (waves.text == "") waves.text = "Отбито волн: 1";
        waves.text = "Отбито волн: " + (Int32.Parse(waves.text.Replace("Отбито волн: ", "")) + 1).ToString();
    }
}
