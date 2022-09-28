using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

public class EnglishGameInput : MonoBehaviour
{
    private GameObject EGInputPanel, EGCheckmark, EGPanel;
    private Sprite[] allAlphabet;
    private GameObject objLetterImage;
    private KeysConverter kc;
    private int wordLenght = 12, typedLenght = 0; 
    private int actualLetter = 0;
    float lastStep, timeBetweenSteps = 0.1f;

    [NonSerialized] public string fullWord = "";
    private List<LetterImages> letters;

    class LetterImages
    {
        public Coroutine coroutinesSet = null, coroutinesErase = null; public GameObject objLetter = null; public Vector3 position = Vector3.zero; public char value = '\0';
    }

    private void Start() 
    {
        //находим дочерние объекты
        EGPanel = transform.Find("EnglishGamePanel").gameObject;
        EGCheckmark = transform.Find("EnglishGamePanel/EGCheckmark").gameObject;
        EGInputPanel = transform.Find("EnglishGamePanel/EGInputPanel").gameObject;
        //находим объекты в resourses
        objLetterImage = Resources.Load<GameObject>("2d_prefabs/EGInpitButton");       
        allAlphabet = Resources.LoadAll<Sprite>("Sprites/Alphabet");  //загружаем алфавит спрайтов
        
        //создаем конвертер из кейкод в стринг
        kc = new KeysConverter();

        letters = new List<LetterImages>();
    }
    public void Update() 
    {
        if (EGPanel.activeSelf == true)
        {
            //если Backspace то удаляем символ
            //lastStep и timeBetweenSteps для вызова метода не чаще 0.1 в секунду, а то вызовется EraseLetter() миллион раз за раз
            if(Input.GetKey(KeyCode.Backspace)) 
            {
                if(Time.time - lastStep > timeBetweenSteps)
                {
                    lastStep = Time.time;
                    EraseLetter();
                }                
            }
            else if (actualLetter < wordLenght)
            {
                //при нажатии на клавишу записываем букву в ячейку
                foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if(Input.GetKeyDown(vKey))
                    {     
                        TypeLetter(vKey);    
                    }
                }
            }
            else
                EGCheckmark.GetComponent<ForCheckmark>().GoRed();
        }
    }

    private void TypeLetter(KeyCode symbol)
    {
        try {
            //конвертируем из KeyCode в char, а потом находим спрайт по такому символу
            //если ошибка конвертации (KeyCode больше одного символа) то вызываем ошибку
            char letter;
            if (symbol == KeyCode.Space)
                letter = ' ';
            else
            {
                string keyChar = kc.ConvertToString(symbol);
                letter = Convert.ToChar(keyChar);
            }

            if(Char.IsLetter(letter) == false)
                return;

            Sprite spriteSymbol = SetSprite(letter);

            while (actualLetter + 1 > letters.Count)
            {
                letters.Add(null);
                letters[letters.Count-1] = new LetterImages();
            }

            //останавливаем и заканчиваем если нужно прошлые куротины
            if(letters[actualLetter].coroutinesSet != null)
            {
                StopCoroutine(letters[actualLetter].coroutinesSet);
                letters[actualLetter].coroutinesSet = null;
            } else if (letters[actualLetter].coroutinesErase  != null)
            {
                StopCoroutine(letters[actualLetter].coroutinesErase );
                Destroy(letters[actualLetter].objLetter);
                letters[actualLetter].objLetter = null;                    
                letters[actualLetter].coroutinesErase  = null;  
            }             

            //создать новый объект в панели EG для записи изображения буквы
            letters[actualLetter].objLetter = Instantiate(objLetterImage, letters[actualLetter].position, Quaternion.Euler(0,0,0));
            letters[actualLetter].objLetter.transform.SetParent(EGInputPanel.transform, false);
            letters[actualLetter].objLetter.transform.localScale = new Vector3 (1f,1f,1f);
            //задаем ему спрайт символа
            letters[actualLetter].objLetter.GetComponent<Image>().sprite = spriteSymbol;

            PushLettersApart(true); //двигаем картинки символов

            TypeEffect(letters[actualLetter].objLetter, actualLetter);   //красиво опускаем его в его позицию    

            letters[actualLetter].value = letter;
            RecountLetters();   //пересчет полного слова

            typedLenght += 1;
            actualLetter += 1;
        } 
        catch(FormatException)
        {
            Debug.Log("введен не символ буквы");
        }
        catch(IndexOutOfRangeException)
        {
            Debug.Log("нет ячейки для записы буквы");
        }
    }
    private void EraseLetter()
    {
        //если текущий символ хотя бы второй, то возвращаемся назад и стираем сивмол там
        if (actualLetter > 0)
        {
            actualLetter -= 1; 
            typedLenght -= 1; 

            letters[actualLetter].value = '\0';
            RecountLetters();   //пересчет полного слова

            //останавливаем и заканчиваем если нужно прошлые куротины
            if(letters[actualLetter].coroutinesSet != null)
            {
                StopCoroutine(letters[actualLetter].coroutinesSet);
                letters[actualLetter].coroutinesSet = null;
                letters[actualLetter].objLetter.transform.localPosition = letters[actualLetter].position;
                letters[actualLetter].objLetter.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            } else if (letters[actualLetter].coroutinesErase  != null)
            {
                StopCoroutine(letters[actualLetter].coroutinesErase );
                letters[actualLetter].coroutinesErase  = null;  
            } 
            
            PushLettersApart(false);    //двигаем картинки символов

            EraseEffect(letters[actualLetter].objLetter, actualLetter); //красиво поднимаем его и удаляем 
        }
    }    

    private void TypeEffect(GameObject objLetter, int actualcoroutine)
    {
        //устанавливаем вектор движения и поворота
        //потом смещаем его чтобы потом опустить куротином
        Vector3 positionEffect = new Vector3(5f,15f,0f);
        Vector3 rotationEffect = new Vector3(0f,0f,-20f);
        objLetter.transform.localPosition = letters[actualcoroutine].position + positionEffect;
        objLetter.transform.localRotation = Quaternion.Euler(objLetter.transform.localRotation.eulerAngles + rotationEffect);    
              
        letters[actualcoroutine].coroutinesSet = StartCoroutine(SetLetterCoroutine(objLetter, positionEffect, rotationEffect, actualcoroutine));  
    }
    private void EraseEffect(GameObject objLetter, int actualcoroutine)
    {
        //устанавливаем вектор движения и поворота
        //потом выставляем ему стартовую позицию чтобы потом поднять куротином
        Vector3 positionEffect = new Vector3(5f,15f,0f);
        Vector3 rotationEffect = new Vector3(0f,0f,-20f);
        objLetter.transform.localPosition = letters[actualcoroutine].position;
        objLetter.transform.localRotation = Quaternion.Euler(objLetter.transform.localRotation.eulerAngles + rotationEffect);           

         
        letters[actualcoroutine].coroutinesErase = StartCoroutine(EraseLetterCoroutine(objLetter, positionEffect, rotationEffect, actualcoroutine));  
    }

    private IEnumerator SetLetterCoroutine(GameObject objLetter, Vector3 positionEffect, Vector3 rotationEffect, int actualcoroutine)
    {
        //двигаем и поворачиваем объект при набирании его, потом устанавливаем ему его позицию и выравниваем rotation (чтобы сел в нужное место)
        float speed = 0.000001f;
        int factor = 20;    //количество кадров для движения
        
        for(int i = 0; i <= factor; i++) 
        {
            yield return new WaitForSeconds(Time.deltaTime*speed);

            objLetter.transform.Translate(positionEffect * (-1) /factor * Time.deltaTime);
            objLetter.transform.Rotate(rotationEffect * (-1) /factor);
        }

        objLetter.transform.localPosition = letters[actualcoroutine].position;
        objLetter.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); 
    }
    private IEnumerator EraseLetterCoroutine(GameObject objLetter, Vector3 positionEffect, Vector3 rotationEffect, int actualcoroutine)
    {
        //двигаем и поворачиваем объект при удалении (обратно), потом удаляем его
        float speed = 0.000001f;
        int factor = 20;    //количество кадров для движения
        
        for(int i = 0; i <= factor; i++) 
        {
            yield return new WaitForSeconds(Time.deltaTime*speed);

            objLetter.transform.Translate(positionEffect /factor * Time.deltaTime);
            objLetter.transform.Rotate(rotationEffect /factor);
        }
        
        Destroy(letters[actualcoroutine].objLetter);
        letters[actualcoroutine].objLetter = null;
    }

    private void PushLettersApart(bool type)
    {
        //двигаем символы при добавлении или удалении одного
        float width = objLetterImage.GetComponent<RectTransform>().sizeDelta.x;
        float indent = 3f;

        if (type == true) //добавление нового символа
        {
            if (typedLenght > 0)
            {
                //двигаем все символы (кроме последнего) влево на половину его ширины и отступа
                for(int i = 0; i < typedLenght; i++) //(typedLenght)/2
                {
                    //Debug.Log("going left");
                    letters[i].position = new Vector3(letters[i].position.x - width/2 - indent/2, letters[i].position.y, letters[i].position.z); 
                }
                /*for(int i = typedLenght/2 + 1; i < typedLenght; i++)
                {
                    Debug.Log("going right");
                    letters[i].position = new Vector3(letters[i].position.x + width/2 + indent/2, letters[i].position.y, letters[i].position.z); 
                }*/

                //позиция последнего символа - на ширину и отступ вправо
                letters[typedLenght].position = new Vector3(letters[typedLenght-1].position.x + width + indent, letters[typedLenght-1].position.y, letters[typedLenght-1].position.z);   
            }
        }
        else //удаление одного символа
        {           
            //двигаем все символы вправо на половину его ширины и отступа 
            for(int i = 0; i < typedLenght; i++)
            {
                letters[i].position = new Vector3(letters[i].position.x + width/2 + indent/2, letters[i].position.y, letters[i].position.z); 
            }  
        }
        //перестраиванием позиции изображения букв по массиву позиций
        for(int i = 0; i < typedLenght; i++)
        {
            letters[i].objLetter.transform.localPosition = letters[i].position;
        }
    }

    private Sprite SetSprite(char letter)
    {
        Sprite spriteSymbol;

        switch (letter)
        {
            case('A'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_0");
                break;
            case('B'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_1");
                break;
            case('C'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_2");
                break;
            case('D'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_3");
                break;
            case('E'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_4");
                break;
            case('F'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_5");
                break;
            case ('G'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_6");
                break;
            case ('H'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_7");
                break;
            case ('I'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_8");
                break;
            case ('J'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_9");
                break;
            case ('K'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_10");
                break;
            case ('L'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_11");
                break;
            case ('M'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_12");
                break;
            case ('N'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_13");
                break;
            case ('O'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_14");
                break;
            case ('P'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_15");
                break;
            case ('Q'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_16");
                break;
            case ('R'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_17");
                break;
            case ('S'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_18");
                break;
            case ('T'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_19");
                break;
            case ('U'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_20");
                break;
            case ('V'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_21");
                break;
            case ('W'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_22");
                break;
            case ('X'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_23");
                break;
            case ('Y'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_24");
                break;
            case ('Z'):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_25");
                break;
            case (' '):
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_26");
                break;
            default:
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_26");
                break;
        }

        return spriteSymbol;
    }
    
    private void RecountLetters()
    {    
        //собираем полное слово из массива char набранноых символов    
        fullWord = "";
        for(int i = 0; i <= typedLenght; i++)
            fullWord += letters[i].value;
    }
    public void Clear()
    //отчисить данные о набранном слове - отчистить его массивы и удаляем объекты символов
    {    
        while (letters.Count != 0)
        {
            Destroy(letters[letters.Count-1].objLetter);
            letters.RemoveAt(letters.Count-1);
        }

        actualLetter = 0; typedLenght = 0;
    }
}
