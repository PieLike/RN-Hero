using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public GameObject infoPanel;
    TMP_Text label, text;
    Camera cam;
    private void Start() 
    {
        cam = Camera.main;

        infoPanel.SetActive(false);

        Transform textObj = infoPanel.transform.Find("Text");
        text = textObj.GetComponent<TMP_Text>();

        Transform labelObj = infoPanel.transform.Find("Label");
        label = labelObj.GetComponent<TMP_Text>();

        //Image panel = infoPanel.GetComponent<Image>();
        //panel.alphaHitTestMinimumThreshold = 0.90f;
    }

    public void ActivateWindow(string _label, string info)
    {
        Vector2 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        infoPanel.transform.position = mouseWorldPosition;

        label.text = _label;
        text.text = info;
        infoPanel.SetActive(true);
    }
    public void HideWindow()
    {
        infoPanel.SetActive(false);
    }
}
