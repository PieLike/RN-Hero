using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class PickerWheelMy : MonoBehaviour
{
	public GameObject wheelPiecePrefab, linePrefab;
    private Transform linesParent, wheelPiecesParent;
	public GameObject indicator; public int indicatorIndex;	
	public float pieceAngle;
	[SerializeField] public List<WheelPiece> wheelPieces;	
	private Camera cam;

	public class WheelPiece 
	{
		public int index ;
		public string text ;			
		public GameObject gameObj;
		public Button button;
		public Sprite image;
			public Potion potion;
			public int count;
   }
   
   public virtual void Start()
   {		
		cam = Camera.main;

		//wheelPiecePrefab = Resources.Load<GameObject>("");
		//linePrefab = Resources.Load<GameObject>("");

		linesParent = transform.Find("Spinning Circle/Lines");
		wheelPiecesParent = transform.Find("Spinning Circle/Wheel Pieces");	
   	}
	
	public void UpdateWheel()
	{
		if (wheelPieces != null && wheelPieces.Count > 0)
		{
			pieceAngle = 360f / wheelPieces.Count;
			//GenerateHight();
			GenerateWheel();
		}
	}	
	private void GenerateWheel() 
	{
		//wheelPiecePrefab = InstantiatePiece() ;

		//RectTransform rect = wheelPiecePrefab.transform.GetChild (0).GetComponent <RectTransform> () ;
		//float pieceWidth = Mathf.Lerp (pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp (piecesMin, piecesMax, wheelPieces.Length)) ;
		//float pieceHeight = Mathf.Lerp (pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp (piecesMin, piecesMax, wheelPieces.Length)) ;
		//rect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, pieceWidth) ;
		//rect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, pieceHeight) ;

		for (int i = 0; i < wheelPieces.Count; i++)
		{
			DrawPiece (i);
		}

		//Destroy (wheelPiecePrefab) ;
	}
	public void ClearWheel()
	{
		if (wheelPieces == null) return;
		foreach (var item in wheelPieces)
		{
			Destroy(item.gameObj);
		}
		wheelPieces.Clear();
	}
	
	private void DrawPiece(int index) 
	{		
        wheelPieces[index].index = index;
		
        GameObject newPiece = InstantiatePiece ();

		var label = newPiece.transform.Find("Label");
		if (label != null)
		{
			label.GetComponent<Text>().text = wheelPieces[index].text;
			label.localRotation = Quaternion.Euler(0,0,(pieceAngle * index));
		}

		var button = newPiece.transform.Find("Button");
		button.GetComponent<Image>().fillAmount = pieceAngle / 360f;
		button.localRotation = Quaternion.Euler(0,0,(180f - pieceAngle / 2f) * (-1));

		var image = newPiece.transform.Find("Image");
		if (image != null && wheelPieces[index].image != null)
		{
			//image.GetComponent<SVGImage>().sprite = wheelPieces[index].image;
			image.GetComponent<Image>().sprite = wheelPieces[index].image;
			image.localRotation = Quaternion.Euler(0,0,(pieceAngle * index));

			if (wheelPieces[index].count != 0)
			{
				var count = image.transform.Find("Count");
				if (count != null)
				{
					count.GetComponent<TMP_Text>().text = "x" + wheelPieces[index].count.ToString();
				}
			}
		}
		
        //Line
        if (wheelPieces.Count > 1)
		{
			Transform lineTransform = Instantiate (linePrefab, linesParent.position, Quaternion.identity, linesParent).transform;
        	lineTransform.RotateAround (linesParent.position, Vector3.back, (pieceAngle * index) + (pieceAngle / 2f) );
		}

        Transform pieceTransform = newPiece.GetComponent<Transform>();
        pieceTransform.RotateAround (wheelPiecesParent.position, Vector3.back, pieceAngle * index);

		wheelPieces[index].button = button.GetComponent<Button>();
		wheelPieces[index].gameObj = newPiece;
    }

    private GameObject InstantiatePiece() 
	{
        return Instantiate (wheelPiecePrefab, wheelPiecesParent.position, Quaternion.identity, wheelPiecesParent) ;
    }

	public virtual void Update() 
	{
		float angle = MyMathCalculations.CalculateAngle2D(transform.position, cam.ScreenToWorldPoint(Input.mousePosition), transform.up) * (-1);
		//Vector2.Lerp(Vector2.zero, cam.ScreenToWorldPoint(Input.mousePosition), 0.5f);
		indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, Quaternion.Euler(0f,0f,angle+180), 1f);

		var corAngle = angle - pieceAngle/2f;
		if (corAngle >= 0 && wheelPieces != null)
		{
			indicatorIndex = (int) (wheelPieces.Count - (corAngle / pieceAngle));
		}
		else
			indicatorIndex = (int) ((corAngle / pieceAngle) * (-1));

		
		//Debug.Log(wheelPieces[indicatorIndex].text);
		/*if (Input.GetMouseButtonDown(0) && englishGameInput != null)
		{
			//Debug.Log(wheelPieces[indicatorIndex].text);
			//wheelPieces[indicatorIndex].button.onClick.Invoke();
			englishGameInput.TypeLetter(wheelPieces[indicatorIndex].text);
		}*/
	}	
}