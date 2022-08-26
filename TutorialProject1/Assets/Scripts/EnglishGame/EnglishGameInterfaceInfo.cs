using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EG_UI", menuName = "UI/EG_UI")]

public class EnglishGameInterfaceInfo : ScriptableObject
{
    [SerializeField] private GameObject MarkCheck;
    public GameObject markCheck => this.MarkCheck;
    
}
