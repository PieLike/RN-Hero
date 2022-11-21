using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

public class Alchemy : MonoBehaviour
{
    //private Word parent = new Word{ name = "pizza", pos = Word.Pos.Noun };
    //private List<Word> sons;
    public int ingredientsCount, allIngredientsCount;   Potion propPotion;
    public List<Word> rightIngredients;
    [NonSerialized] public List<Word> allIngredients, propIngredients;
    [NonSerialized] public DictionaryManager dictionaryManager; private PotionManager potionManager;
    [NonSerialized] public InterfaceManager interfaceManager;   AlcScroll alcScroll; PotionsScroll potionsScroll;
    private Animator animatorPot, animatorPotions; 
    private static Image progressLine; private float smoothTime = 0.25f, Velocity = 0.0f;
    private bool canClose;

    private void OnEnable()
    {
        dictionaryManager = FindObjectOfType<DictionaryManager>();  
        interfaceManager = FindObjectOfType<InterfaceManager>(); 
        potionManager = FindObjectOfType<PotionManager>(); 

        interfaceManager.AlcExit.GetComponent<Button>().onClick.AddListener(CloseAlcGame); 
        interfaceManager.APExit.GetComponent<Button>().onClick.AddListener(CloseAlcGame);
        interfaceManager.AlcBack.GetComponent<Button>().onClick.AddListener(BackToPotionWindow);

        potionsScroll = gameObject.GetComponent<PotionsScroll>(); 
        alcScroll = gameObject.GetComponent<AlcScroll>(); ;

        animatorPot = interfaceManager.AlcPanel.GetComponent<Animator>();
        animatorPotions = interfaceManager.AlcPotionPanel.GetComponent<Animator>();
        //ChangeWindow(false);   

        allIngredients = new List<Word>();
        propIngredients = new List<Word>();
        if (rightIngredients == null)
            rightIngredients = new List<Word>();

        progressLine = interfaceManager.AlcProgressLine.GetComponent<Image>();

        //MathLevel();
        //ChoseAllIngredientsAndMainIngredients();
    }

    private void ChoseAllIngredientsAndMainIngredients()
    {
        int[] randomIndex = ChoseAllIngredients();
        if (randomIndex != null && ingredientsCount <= allIngredientsCount)
        {
            ChoseIngredients(randomIndex);
            alcScroll.FillScroll();
        }
        else
            Debug.Log("randomIndex = null");
    }

    private void ChoseIngredients(int[] randomIndex)
    {
        if (ingredientsCount > 0 && dictionaryManager.words.Count > 0 && dictionaryManager.words.Count >= ingredientsCount)
        {
            for (var i = 0; i < ingredientsCount; i++)            
            {
                rightIngredients.Add(dictionaryManager.words[randomIndex[i]]);
                //allIngredients.Remove(dictionaryManager.words[randomIndex[i]]);
            }  
            FillIngredientsOnInterface();            
        }
    }

    private int[] ChoseAllIngredients()
    {
        if (allIngredientsCount > 0 && dictionaryManager.words.Count > 0)   //&& dictionaryManager.words.Count >= allIngredientsCount
        {
            int[] randomIndex = new int[(dictionaryManager.words.Count >= allIngredientsCount) ? allIngredientsCount : dictionaryManager.words.Count];//new int[allIngredientsCount];
            randomIndex = GetRandomMultiply(0, dictionaryManager.words.Count, randomIndex.Length); //(0, dictionaryManager.words.Count, allIngredientsCount);

            foreach (int index in randomIndex)
            {
                Word word = dictionaryManager.words[index];
                if (word.translate != null && word.translate.Count != 0)
                    allIngredients.Add(word);
            } 
            return randomIndex;
        } 
        else
        {

        }     
        return null;  
    }

    public void AddToProps(Word word)
    {
        if(rightIngredients.Contains(word) == false)
        {
            WrongIngredient();
            return;
        }   
        RightIngredient(word);
    }
    private bool CheckProps()
    {
        IEnumerable<Word> wrongsIngredients = rightIngredients.Except(propIngredients);
		if (wrongsIngredients != null && wrongsIngredients.Count() > 0)
			return false;
        else
            return true;            
    }

    private int[] GetRandomMultiply(int from, int to, int lenght)
    {
        int[] randoms = new int[lenght];
        for (var i = 0; i < randoms.Length; i++)
        {
            randoms[i] = -1;
        }

        for (var i = 0; i < lenght; i++)
        {
            randoms[i] = GetRadnom(from, to, randoms);
        }
        return randoms;
    }    

    private int GetRadnom(int from, int to, int[] randoms)
    {
        int random = UnityEngine.Random.Range(from, to);
        foreach (int rand in randoms)
        {
            if(rand != (-1) && random == rand)
            {
                return GetRadnom(from, to, randoms);
            }
        }
        return random;
    }

    private void WrongIngredient()
    {
        //Debug.Log("Wrong Ingredient");

        CloseAlcGame();
        interfaceManager.messageScript.NewMessage("Провал!", "Зелье не получилось");
        //ResetAlcGame();
    }
    private void RightIngredient(Word word)
    {
        //Debug.Log("Right Ingredient");

        propIngredients.Add(word);

        if (rightIngredients.Count == propIngredients.Count)
        {
            if (CheckProps() == true)
                PropIngredientRight();
        }
    }

    private void PropIngredientRight()
    {
        //Debug.Log("All Ingredient are Right");

        potionManager.AddActivePotion(propPotion);
        //CloseAlcGame();
        canClose = true;
        interfaceManager.messageScript.NewMessage("Успех!", "Вы создали зелье " + propPotion.potionName);

        Experience.AddExp(MainVariables.expForPotionCreate);
    }
    public void SetPotion(Potion potion)
    {
        propPotion = potion;

        if (potion != null)
        {
            Image svgImage = interfaceManager.AlcPotionImage.GetComponent<Image>();
            svgImage.sprite = potion.image;
        }
        else
            Debug.LogError("potion = null");
    }

    public void ResetAlcGame()
    {
        if (CanAddNewPotion())
        {
            allIngredients.Clear();
            propIngredients.Clear();
            rightIngredients.Clear();

            alcScroll.ClearScroll();

            MathLevel();
            ChoseAllIngredientsAndMainIngredients();

            progressLine.fillAmount = 0;
            canClose = false;
        }
        else
        {
            interfaceManager.messageScript.NewMessage("Нет места", "Вы не можете нести больше зелий");
            CloseAlcGame();
        }
    }
    private bool CanAddNewPotion()
    {
        int potionWeightCount = 0;        
        if (potionManager.activePotions != null)
        {
            foreach (var item in potionManager.activePotions)
            {
                potionWeightCount += item.count;
            }
            if (HeroMainData.maxPotionWithSelf <= potionWeightCount)
                return false;
        }        

        return true;
    }

    private void FillIngredientsOnInterface()
    {
        if (rightIngredients != null)
        {
            var ingrList = interfaceManager.Ingredients.GetComponent<TMP_Text>();
            
            ingrList.text = "";
            foreach (var item in rightIngredients)
            {
                if (string.IsNullOrEmpty(ingrList.text))
                    ingrList.text = item.word;
                else
                    ingrList.text += ", " + item.word;
            }
        }
        else
        {
            Debug.Log("rightIngredients = null");
            return;
        }
    }

    private void MathLevel()
    {
        Voculabrary.ReCountWordActualCount();
        if (HeroMainData.wordActualCount > 1)
        {
            int maxCount = HeroMainData.wordActualCount >= 10 ? 10 : HeroMainData.wordActualCount;
            allIngredientsCount = HeroMainData.level >= 10 ? maxCount : HeroMainData.level + 1;
            
            ingredientsCount = (int) Math.Round((double) (allIngredientsCount / 3));
            if (ingredientsCount == 0)
                ingredientsCount = 1;
        }
        else
        {
            interfaceManager.messageScript.NewMessage("Недостаточно слов", "Добудьте больше слов для использования алхимии");
            CloseAlcGame();
        }
    }

    public void OpenAlcGame()
    {       
        OnGameStartAndUpdate.CloseInterface();

        interfaceManager.AlcPanel.SetActive(true);
        interfaceManager.AlcPotionPanel.SetActive(true); 
        
        ChangeWindow(false);
        animatorPotions.Play("WindowStayCenter");
        animatorPot.Play("WindowStayRight");

        //MainVariables.inInterface = true;
        Time.timeScale = 0f; 
        //ResetAlcGame();

        OnGameStartAndUpdate.OnInterfaceClose += CloseAlcGame;
    }
    private void CloseAlcGame()
    {        
        potionsScroll.ClearScroll();
        interfaceManager.AlcPanel.SetActive(false);
        interfaceManager.AlcPotionPanel.SetActive(false); 
        //MainVariables.inInterface = false;
        Time.timeScale = 1f;

        progressLine.fillAmount = 0;
        canClose = false;

        OnGameStartAndUpdate.OnInterfaceClose -= CloseAlcGame;
    }

    public void AlchemyButton()
    {
        if(interfaceManager.AlcPanel.activeSelf == false || interfaceManager.AlcPotionPanel.activeSelf == false)
        {
            OpenAlcGame();            
        }
        else
        {
            CloseAlcGame();
        }
    }
    
    public void ChangeWindow(bool pot)
    {
        if (pot)
        {
            animatorPotions.SetInteger("CenterLeftRight", 1);
            animatorPot.SetInteger("CenterLeftRight", 0);
            //interfaceManager.AlcPanel.transform.position = new Vector3 (0f, interfaceManager.AlcPanel.transform.position.y, interfaceManager.AlcPanel.transform.position.z);
            //interfaceManager.AlcPotionPanel.transform.position = new Vector3 (-800f, interfaceManager.AlcPotionPanel.transform.position.y, interfaceManager.AlcPotionPanel.transform.position.z);
        }
        else
        {
            animatorPotions.SetInteger("CenterLeftRight", 0);
            animatorPot.SetInteger("CenterLeftRight", 2);
            //interfaceManager.AlcPanel.transform.position = new Vector3 (800f, interfaceManager.AlcPanel.transform.position.y, interfaceManager.AlcPanel.transform.position.z);
            //interfaceManager.AlcPotionPanel.transform.position = new Vector3 (0f, interfaceManager.AlcPotionPanel.transform.position.y, interfaceManager.AlcPotionPanel.transform.position.z);
        }
    }
    public void BackToPotionWindow()
    {
        ChangeWindow(false);
    }

    void Update()
    {
        //change progress bar
        if (propIngredients.Count != 0 && rightIngredients.Count != 0)
        {
            float hpUpdate = propIngredients.Count / rightIngredients.Count;
            if (progressLine.fillAmount != hpUpdate)
            {
                float smoothUpdate = Mathf.SmoothDamp(progressLine.fillAmount, hpUpdate, ref Velocity, smoothTime,100,Time.unscaledDeltaTime);   
                progressLine.fillAmount = smoothUpdate;
            }
        }
        
        if (progressLine.fillAmount > 0.95f && canClose)
            CloseAlcGame();
    }    
}
