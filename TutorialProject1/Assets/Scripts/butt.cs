using UnityEngine;
using UnityEngine.UI;

public class butt : MonoBehaviour
{    
    [SerializeField] private Sprite sprite;
    void Start()
    {
        Image theButton = GetComponent<Image>();
        theButton.sprite = sprite;
        theButton.alphaHitTestMinimumThreshold = 0.1f;
    }
}
