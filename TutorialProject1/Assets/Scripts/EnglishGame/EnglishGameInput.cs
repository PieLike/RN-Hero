using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Windows.Forms;

public class EnglishGameInput : MonoBehaviour
{
    private GameObject EGInputPanel, EGCheckmark, EGPanel;
    private Sprite[] allAlphabet;
    private GameObject[] letterImages;
    private GameObject objLetterImage;
    private Vector3[] letterPositions;
    private char[] letterValue;
    private KeysConverter kc;
    private int wordLenght = 7, typedLenght = 0; 
    private int actualLetter = 0;
    float lastStep, timeBetweenSteps = 0.1f;
    private Coroutine[] coroutinesSet, coroutinesErase;
    [NonSerialized] public string fullWord = "";

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
        //описываем массив куротин, а также объектов букв и их позиций, массив позиций заполняем (0,0,0)
        coroutinesSet = new Coroutine[wordLenght];
        coroutinesErase = new Coroutine[wordLenght];
        letterImages = new GameObject[wordLenght];
        letterPositions = new Vector3[wordLenght];
        for(int i = 0; i < wordLenght; i++)
            letterPositions[i] = Vector3.zero;
        letterValue = new char[wordLenght];    
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
            string keyChar = kc.ConvertToString(symbol);
            char letter = Convert.ToChar(keyChar);
            Sprite spriteSymbol = SetSprite(letter);

            //останавливаем и заканчиваем если нужно прошлые куротины
            if(coroutinesSet[actualLetter] != null)
            {
                StopCoroutine(coroutinesSet[actualLetter]);
                coroutinesSet[actualLetter] = null;
            } else if (coroutinesErase[actualLetter] != null)
            {
                StopCoroutine(coroutinesErase[actualLetter]);
                Destroy(letterImages[actualLetter]);
                letterImages[actualLetter] = null;                    
                coroutinesErase[actualLetter] = null;  
            }             

            //создать новый объект в панели EG для записи изображения буквы
            letterImages[actualLetter] = Instantiate(objLetterImage, letterPositions[actualLetter], Quaternion.Euler(0,0,0));
            letterImages[actualLetter].transform.SetParent(EGInputPanel.transform, false);
            letterImages[actualLetter].transform.localScale = new Vector3 (1f,1f,1f);
            //задаем ему спрайт символа
            letterImages[actualLetter].GetComponent<Image>().sprite = spriteSymbol;

            PushLettersApart(true); //двигаем картинки символов

            TypeEffect(letterImages[actualLetter], actualLetter);   //красиво опускаем его в его позицию    

            letterValue[actualLetter] = letter;
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

            letterValue[actualLetter] = '\0';
            RecountLetters();   //пересчет полного слова

            //останавливаем и заканчиваем если нужно прошлые куротины
            if(coroutinesSet[actualLetter] != null)
            {
                StopCoroutine(coroutinesSet[actualLetter]);
                coroutinesSet[actualLetter] = null;
                letterImages[actualLetter].transform.localPosition = letterPositions[actualLetter];
                letterImages[actualLetter].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            } else if (coroutinesErase[actualLetter] != null)
            {
                StopCoroutine(coroutinesErase[actualLetter]);
                coroutinesErase[actualLetter] = null;  
            } 
            
            PushLettersApart(false);    //двигаем картинки символов

            EraseEffect(letterImages[actualLetter], actualLetter); //красиво поднимаем его и удаляем 
        }
    }    

    private void TypeEffect(GameObject objLetter, int actualcoroutine)
    {
        //устанавливаем вектор движения и поворота
        //потом смещаем его чтобы потом опустить куротином
        Vector3 positionEffect = new Vector3(5f,15f,0f);
        Vector3 rotationEffect = new Vector3(0f,0f,-20f);
        objLetter.transform.localPosition = letterPositions[actualcoroutine] + positionEffect;
        objLetter.transform.localRotation = Quaternion.Euler(objLetter.transform.localRotation.eulerAngles + rotationEffect);    
              
        coroutinesSet[actualcoroutine] = StartCoroutine(SetLetterCoroutine(objLetter, positionEffect, rotationEffect, actualcoroutine));  
    }
    private void EraseEffect(GameObject objLetter, int actualcoroutine)
    {
        //устанавливаем вектор движения и поворота
        //потом выставляем ему стартовую позицию чтобы потом поднять куротином
        Vector3 positionEffect = new Vector3(5f,15f,0f);
        Vector3 rotationEffect = new Vector3(0f,0f,-20f);
        objLetter.transform.localPosition = letterPositions[actualcoroutine];
        objLetter.transform.localRotation = Quaternion.Euler(objLetter.transform.localRotation.eulerAngles + rotationEffect);           

         
        coroutinesErase[actualcoroutine] = StartCoroutine(EraseLetterCoroutine(objLetter, positionEffect, rotationEffect, actualcoroutine));  
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

        objLetter.transform.localPosition = letterPositions[actualcoroutine];
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
        
        Destroy(letterImages[actualcoroutine]);
        letterImages[actualcoroutine] = null;
    }

    private void PushLettersApart(bool type)
    {
        //двигаем символы при добавлении или удалении одного
        float width = objLetterImage.GetComponent<RectTransform>().sizeDelta.x;
        float indent = 5f;

        if (type == true) //добавление нового символа
        {
            if (typedLenght > 0)
            {
                //двигаем все символы (кроме последнего) влево на половину его ширины и отступа
                for(int i = 0; i < typedLenght; i++) //(typedLenght)/2
                {
                    //Debug.Log("going left");
                    letterPositions[i] = new Vector3(letterPositions[i].x - width/2 - indent/2, letterPositions[i].y, letterPositions[i].z); 
                }
                /*for(int i = typedLenght/2 + 1; i < typedLenght; i++)
                {
                    Debug.Log("going right");
                    LetterPositions[i] = new Vector3(LetterPositions[i].x + width/2 + indent/2, LetterPositions[i].y, LetterPositions[i].z); 
                }*/

                //позиция последнего символа - на ширину и отступ вправо
                letterPositions[typedLenght] = new Vector3(letterPositions[typedLenght-1].x + width + indent, letterPositions[typedLenght-1].y, letterPositions[typedLenght-1].z);   
            }
        }
        else //удаление одного символа
        {           
            //двигаем все символы вправо на половину его ширины и отступа 
            for(int i = 0; i < typedLenght; i++)
            {
                letterPositions[i] = new Vector3(letterPositions[i].x + width/2 + indent/2, letterPositions[i].y, letterPositions[i].z); 
            }  
        }
        //перестраиванием позиции изображения букв по массиву позиций
        for(int i = 0; i < typedLenght; i++)
        {
            letterImages[i].transform.localPosition = letterPositions[i];
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
            default:
                spriteSymbol = allAlphabet.Single(s => s.name == "Alphabet_43");
                break;
        }

        return spriteSymbol;
    }
    
    private void RecountLetters()
    {    
        //собираем полное слово из массива char набранноых символов    
        fullWord = "";
        for(int i = 0; i <= typedLenght; i++)
            fullWord += letterValue[i];
    }
    public void Clear()
    //отчисить данные о набранном слове - отчистить его массивы и удаляем объекты символов
    {    
        for(int i = 0; i < wordLenght; i++)
        {
            if (letterImages[i] != null)
            {
                Destroy(letterImages[i]);
                letterImages[i] = null;
            }
        }
        for(int i = 0; i < wordLenght; i++)
            coroutinesSet[i] = null;
        for(int i = 0; i < wordLenght; i++)
            coroutinesErase[i] = null;        
        for(int i = 0; i < wordLenght; i++)
            letterPositions[i] = Vector3.zero;
        for(int i = 0; i < wordLenght; i++)
            letterValue[i] = '\0';
    }
}
