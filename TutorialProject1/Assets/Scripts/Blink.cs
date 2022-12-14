using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using System;

public class Blink : MonoBehaviour
{
    UnityEngine.Color originColor;
    private void Start() 
    {
        originColor = gameObject.GetComponent<SpriteRenderer>().color;
    }
    public void DoBlink(bool hideObject)
    {
        //снимаем прошлый куротин, чтобы сбросить таймер исчезновения через 3 секунды
        StopCoroutine(SetActiveSetFalseCoroutine());
        //показываем объект (щит) мигаем белым на 0.1 секунды, и запускаем куротину на скрытие объекта (щита) через 3 секунды
        if (hideObject) gameObject.SetActive(true);
        StartCoroutine(BlinkCoroutine());
        if (hideObject) StartCoroutine(SetActiveSetFalseCoroutine());        
    }
    
    private IEnumerator BlinkCoroutine()
    {
        //запоминаем первоначальный цвет и меняем его делая светлее
        //через 0.1 секунд возвращаем первоначальный цвет                 
        UnityEngine.Color newColor = new Color(1f, 0.5f, 0.5f); //ChangeColorBrightness(originColor,10f);
        gameObject.GetComponent<SpriteRenderer>().color = newColor;        
        float duration = 0.1f;

        yield return new WaitForSeconds(duration);    
        gameObject.GetComponent<SpriteRenderer>().color = originColor;    
    }

    private IEnumerator SetActiveSetFalseCoroutine()
    {
        //ждем 3 секунды и скрываем объект (щит)
        float duration = 3.0f;
        yield return new WaitForSeconds(duration); 
        gameObject.SetActive(false);       
    }

    /*public UnityEngine.Color ChangeColorBrightness(UnityEngine.Color color, float correctionFactor)
    {
        //делаем цвет ярче (на данный момент превращаем в белый)
        float red = (float)color.r;
        float green = (float)color.g;
        float blue = (float)color.b;

        if (correctionFactor < 0)
        {
            correctionFactor = 1 + correctionFactor;
            red *= correctionFactor;
            green *= correctionFactor;
            blue *= correctionFactor;
            //Debug.Log("1 correctionFactor = " + correctionFactor);  
        }
        else
        {
            red = (255 - red) * correctionFactor + red;
            green = (255 - green) * correctionFactor + green;
            blue = (255 - blue) * correctionFactor + blue;
            //Debug.Log("2 correctionFactor = " + correctionFactor);  
        }

        return new UnityEngine.Color((int)red, (int)green, (int)blue);;
    }*/
}
