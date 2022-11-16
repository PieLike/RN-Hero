using UnityEngine;

[CreateAssetMenu(menuName = "Status", fileName = "status_")]
public class StatusObject : ScriptableObject
{
    public enum StatusType{ buff, debuff, spell}
    [SerializeField] public string statusName;
    [SerializeField] public Sprite image;
    [SerializeField] public StatusType type;
}
