using UnityEngine;

[System.Serializable]
public class Word
{
    public string _name; public string name {get{ return _name;} set{ _name = value.ToLower();}}
}
