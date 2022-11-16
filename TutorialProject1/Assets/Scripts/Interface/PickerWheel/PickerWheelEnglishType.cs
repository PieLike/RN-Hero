using System.Collections.Generic;
using UnityEngine;

public class PickerWheelEnglishType : PickerWheelMy
{
    private InterfaceManager interfaceManager; EnglishGameInput englishGameInput;
    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";	
    public override void Start() 
    {
        base.Start();

        interfaceManager = FindObjectOfType<InterfaceManager>();
		englishGameInput = interfaceManager.EnglishGame.GetComponent<EnglishGameInput>();
    }

    public override void Update() 
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && englishGameInput != null)
		{
			englishGameInput.TypeLetter(wheelPieces[indicatorIndex].text);
		}
    }

    public void FillWheelPieces(string word)
	{
		List<string> piecesText = SeparateWordAndAddLetters(word.ToUpper(), 3);
		piecesText = ShaffleList(piecesText);

        if (wheelPieces == null)
            wheelPieces = new List<WheelPiece>();

		foreach (var item in piecesText)
		{
			wheelPieces.Add(new WheelPiece{ text = item});
		}
		
		/*wheelPieces.Add(new WheelPiece{ text = "A"});
		wheelPieces.Add(new WheelPiece{ text = "B"});
		wheelPieces.Add(new WheelPiece{ text = "C"});
		wheelPieces.Add(new WheelPiece{ text = "D"});
		wheelPieces.Add(new WheelPiece{ text = "E"});
		wheelPieces.Add(new WheelPiece{ text = "F"});
		wheelPieces.Add(new WheelPiece{ text = "G"});
		wheelPieces.Add(new WheelPiece{ text = "H"});
		wheelPieces.Add(new WheelPiece{ text = "I"});
		//wheelPieces.Add(new WheelPiece{ text = "J"});
		//wheelPieces.Add(new WheelPiece{ text = "K"});
		//wheelPieces.Add(new WheelPiece{ text = "L"});*/
		UpdateWheel();
	}

    private List<string> SeparateWordAndAddLetters(string word, int letters = 0)
	{
		List<string> list = new List<string>();
		foreach (char ch in word)  
		{  
			list.Add(ch.ToString());  
		}
		for (var i = 0; i < letters; i++)
		{
			//string ch = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Range(0, list.Count)]).ToArray());
			int index = Random.Range(0, chars.Length);
			list.Add(chars[index].ToString()); 
		}

		return list;
	}
	private List<string> ShaffleList(List<string> list)
	{
		for (int i = list.Count - 1; i >= 1; i--)
		{
			int j = Random.Range(0, list.Count);

			var temp = list[j];
			list[j] = list[i];
			list[i] = temp;
		}

		return list;
	}
}
