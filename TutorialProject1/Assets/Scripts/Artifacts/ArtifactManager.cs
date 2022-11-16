using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager instance;

    public List<ArtifactInManager> artifacts;
    public class ArtifactInManager
    {
        public Artifact item;
        public bool enabled;
    }

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);        
    }    

    public void AddArtifact(Artifact _item, bool enable)
    {
        if (CheckExisting(artifacts, _item) != (-1)) return;

        ArtifactInManager artifact = new ArtifactInManager{ item = _item, enabled = enable};

        if (artifacts == null) artifacts = new List<ArtifactInManager>();
        artifacts.Add(artifact);

        if (enable)
        {
            EnableOrDisableArtifact(_item, true);
        }
    }


    public void EnableOrDisableArtifact(Artifact item, bool enable)
    {
        if (item.functionList != null)
        {
            foreach (Artifact.Functions func in item.functionList)
            {
                switch (func)
                {
                    case Artifact.Functions.canMeleeAttack:
                        MainVariables.canMeleeAttack = enable; break;
                    default:
                        break;
                }                
            }
        }
        if (item.statsList != null)
        {
            int factor = enable ? 1 : -1 ;
            foreach (Artifact.StatBuff stat in item.statsList)
            {
                switch (stat.stat)
                {
                    case Artifact.Stats.maxHP:
                        HeroMainData.buffMaxHP += stat.count * factor; break;
                    case Artifact.Stats.damage:
                        HeroMainData.buffDamage += stat.count * factor; break;
                    case Artifact.Stats.speed:
                        HeroMainData.buffSpeed += stat.count * factor; break;
                    case Artifact.Stats.attackSpeed:
                        HeroMainData.buffSpeedAttack += stat.count * factor; break;
                    default:
                        break;
                }                
            }
        }
    }

    public void MakeFullReMath()
    {
        if (artifacts != null)
        {
            foreach (var item in artifacts)
            {
                EnableOrDisableArtifact(item.item, item.enabled);
            }
        }
    }

    private int CheckExisting(List<ArtifactInManager> list, Artifact item)
    {
        if (list == null) return (-1);
        return list.FindIndex( delegate(ArtifactInManager artifact){ return artifact.item.artName == item.artName; } );
    }
}
