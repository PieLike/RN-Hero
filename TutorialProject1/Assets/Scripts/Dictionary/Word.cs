using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Word
{
    public enum Pos{Noun, Verb, Adjective, Determiner, Adverb, Pronoun, Article, Conjunction, Interjection, Preposition, Numeral, Not, To, These, none}
    public string _name; public string word {get{ return _name;} set{ _name = value.ToLower();}}
    public int learnCount;  public bool learned;//полностью
    
    public List<string> translate, addictionTranslate, addictionTranslate2;
    public Pos pos;
    public int frequency;
    public bool colloq, filled;

    public Sprite icon;  //Image
}
