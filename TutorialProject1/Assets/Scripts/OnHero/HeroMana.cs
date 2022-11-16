using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroMana : MonoBehaviour
{
    private InterfaceManager interfaceManager;
    private static Image mpLine; private float smoothTime = 0.25f, Velocity = 0.0f; TMP_Text mpNumber;
    private static float prevMp;
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
            mpLine = interfaceManager.MpLine.GetComponent<Image>();
            mpNumber = interfaceManager.MpNumber.GetComponent<TMP_Text>();

        HeroMainData.currentMP = HeroMainData.plainMaxMP + HeroMainData.buffMaxMP;

        UpdateMp();
    }
    private static void UpdateMp()
    {
        if (HeroMainData.currentMP != prevMp)
        {
            prevMp = HeroMainData.currentMP;
        } 
    }

    public void RecoverMp (float points)
    {
        HeroMainData.currentMP += points;
        UpdateMp();
    }

    void Update()
    {
        float mpUpdate = HeroMainData.currentMP / (HeroMainData.plainMaxMP + HeroMainData.buffMaxMP);
        if (mpLine.fillAmount != mpUpdate)
        {
            float smoothMpUpdate = Mathf.SmoothDamp(mpLine.fillAmount, mpUpdate, ref Velocity, smoothTime);   
            mpLine.fillAmount = smoothMpUpdate;

            //mpNumber.text = HeroMainData.currentMP.ToString() + "/" + (HeroMainData.plainMaxMP + HeroMainData.buffMaxMP).ToString();
        }
    }
}
