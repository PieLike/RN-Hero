using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private GameObject statusPrefab; List<StatusIcon> StatusesList;    
    private Sprite spellFrameSprite, buffFrameSprite, debuffFrameSprite, anotherFrameSprite;

    public class StatusIcon 
    {
        public float timeDisable, timeEnable;
        public GameObject statusObject;
        public StatusObject data;
        public bool reloadUpdate;
        public Image mask;
    }
    public class StatusFrame
    {
        StatusObject.StatusType type;
        GameObject prefub;
    }

    private void Start() 
    {
        statusPrefab = Resources.Load<GameObject>("2d_prefabs/Status");
        StatusesList = new List<StatusIcon>(); 

        spellFrameSprite = Resources.Load<Sprite>("SVGs/SpellFrame");
        buffFrameSprite = Resources.Load<Sprite>("SVGs/BuffFrame");
        debuffFrameSprite = Resources.Load<Sprite>("SVGs/DebuffFrame");
        anotherFrameSprite = Resources.Load<Sprite>("SVGs/AnotherFrame");
    }
    public void AddBuff(StatusObject status, float duration)
    {
        StatusIcon statusIcon = FindStatus(status);
        if(statusIcon == null)
        {
            statusIcon = new StatusIcon();

            statusIcon.data = status;
            statusIcon.statusObject = Instantiate(statusPrefab, transform);
            
            Transform maskObj = statusIcon.statusObject.transform.Find("Mask");
                statusIcon.mask = maskObj.GetComponent<Image>();
                Transform iconObj = maskObj.transform.Find("Icon");
                    SVGImage icon = iconObj.GetComponent<SVGImage>();
                        icon.sprite = status.image;  

            //frame
            Transform frameObj = statusIcon.statusObject.transform.Find("Frame");
                SVGImage frame = frameObj.GetComponent<SVGImage>();
            switch (status.type)
            {
                case StatusObject.StatusType.spell:
                    frame.sprite = spellFrameSprite;
                    break;
                case StatusObject.StatusType.buff:
                    frame.sprite = buffFrameSprite;
                    break;
                case StatusObject.StatusType.debuff:
                    frame.sprite = debuffFrameSprite;
                    break;
                default:
                    break;
            }
            

            EnableStatus(statusIcon, duration);

            StatusesList.Add(statusIcon);
        }
        else
        {
            UpdateStatus(statusIcon, duration);
        }
    }    

    private void EnableStatus(StatusIcon status, float duration)
    {
        status.timeDisable = Time.time + duration;   //status.timeDisable == 0f ? Time.time + duration : status.timeDisable + duration;
        status.timeEnable = Time.time;   //status.timeDisable == 0f ? Time.time : status.timeEnable;

        status.reloadUpdate = true;
    }
    private void UpdateStatus(StatusIcon status, float duration)
    {
        status.timeDisable = status.timeDisable + duration;
        status.timeEnable = Time.time;
    }      

    void Update()
    {
        foreach (var status in StatusesList)
        {
            if (status.reloadUpdate)
            {
                float remaining = status.timeDisable - Time.time;
                if (remaining >= 0) 
                {
                    float total = status.timeDisable - status.timeEnable;
                    if (total > 0)
                    {
                        status.mask.fillAmount = remaining / total;
                    }
                }
                else
                {
                    status.reloadUpdate = false; 
                    DisableStatus(status);
                    break;
                }
            }
        }
    }

    private void DisableStatus(StatusIcon status)
    {
        Destroy(status.statusObject);
        StatusesList.Remove(status);
    }

    private StatusIcon FindStatus(StatusObject _status)
    {
        return StatusesList.Find( delegate(StatusIcon status){ return status.data == _status; } );
    }

    public void ClearScroll()   //выполнять на выходе
    {
        foreach (var item in StatusesList)
        {
            Destroy(item.statusObject);
        }
        StatusesList.Clear();
    }
}
