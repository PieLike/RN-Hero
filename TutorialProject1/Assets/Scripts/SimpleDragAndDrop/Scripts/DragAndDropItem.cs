using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using Unity.VectorGraphics;

/// <summary>
/// Drag and Drop item.
/// </summary>
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public enum ImageType {svg, png}; public ImageType imageType;
	public Image myImagePng; public SVGImage myImageSvg; Vector2 defaultSpriteScale = new Vector2 (35f,35f); float multiplier;
	public bool SpriteFromWord;
	public static bool dragDisabled = false;										// Drag start global disable

	public static DragAndDropItem draggedItem;                                      // Item that is dragged now
	public static GameObject icon;                                                  // Icon of dragged item
	public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

	public delegate void DragEvent(DragAndDropItem item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	private static Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                   		// Name of canvas
	private static int canvasSortOrder = 100;										// Sort order for canvas

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (canvas == null)
		{
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;
		}
		myImagePng = GetComponent<Image>();
		if (myImagePng != null)
		{
			imageType = ImageType.png;			
		}
		else
		{
			myImageSvg = GetComponent<SVGImage>();
			if (myImageSvg != null)
			{
				imageType = ImageType.svg;
			}
			else
				Debug.LogError("Не удается найти компонент Image или SVGImage у предмета");
		}
	}

	/// <summary>
	/// This item started to drag.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (dragDisabled == false)
		{
			sourceCell = GetCell();                       							// Remember source cell
			draggedItem = this;                                             		// Set as dragged item
			// Create item's icon
			icon = new GameObject();
			icon.transform.SetParent(canvas.transform);
			icon.name = "Icon";

			if (imageType == ImageType.png)
			{
				//Image myImage = GetComponent<Image>();				
				myImagePng.raycastTarget = false;                                        	// Disable icon's raycast for correct drop handling
				Image iconImage = icon.AddComponent<Image>();
				iconImage.raycastTarget = false;
				if (SpriteFromWord)
				{
					WordData wordData = GetComponent<WordData>();
					if (wordData != null && wordData.word.icon != null)
					{
						iconImage.sprite = wordData.word.icon;
					}
				}
				else
					iconImage.sprite = myImagePng.sprite;

				multiplier = (float)(Screen.width / 800f);
			}
			else
			{
				myImageSvg.raycastTarget = false;                                        	// Disable icon's raycast for correct drop handling
				SVGImage iconImage = icon.AddComponent<SVGImage>();
				iconImage.raycastTarget = false;
				iconImage.preserveAspect = true;
				if (SpriteFromWord)
				{
					WordData wordData = GetComponent<WordData>();
					if (wordData != null && wordData.word.icon != null)
						iconImage.sprite = wordData.word.icon;
				}
				else
					iconImage.sprite = myImageSvg.sprite;

				multiplier = (float)(Screen.width / 800f);
			}

			RectTransform iconRect = icon.GetComponent<RectTransform>();
			// Set icon's dimensions
			RectTransform myRect = GetComponent<RectTransform>();
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			
			//iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);			
			iconRect.sizeDelta = new Vector2(defaultSpriteScale.x, defaultSpriteScale.y) * multiplier;

			if (OnItemDragStartEvent != null)
			{
				OnItemDragStartEvent(this);                                			// Notify all items about drag start for raycast disabling
			}

			if (sourceCell.cellType2 == DragAndDropCell.CellType2.alcSlot)
			{
				//Destroy(this.gameObject);
				myImagePng.color = Color.clear;
			}
		}
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (icon != null)
		{
			icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
		}
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		ResetConditions();
	}

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	private void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                       		// Notify all cells about item drag end
		}

		if (sourceCell != null && sourceCell.cellType2 == DragAndDropCell.CellType2.alcSlot)
		{
			myImagePng.color = Color.white;
		}

		draggedItem = null;
		icon = null;
		sourceCell = null;		
	}

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		if (imageType == ImageType.png)
		{
			Image image = GetComponent<Image>();
			if (image != null)
			{
				image.raycastTarget = condition;
			}
		}
		else
		{
			SVGImage image = GetComponent<SVGImage>();
			if (image != null)
			{
				image.raycastTarget = condition;
			}
		}
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropCell GetCell()
	{
		return GetComponentInParent<DragAndDropCell>();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}
}
