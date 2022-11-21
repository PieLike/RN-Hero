using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Breakable : MonoBehaviour
{   
    string areaName; Vector3Int positionOnTileMap;
    Transform tileLayer; Tilemap tilemap; TileBase tileBase;
    ParticleSystem particleSystemDamage; Transform particleSys;
    private void Start() 
    {
        try
        {
            tileLayer = transform.parent;
            Transform Grid      = tileLayer.parent;
            Transform Area      = Grid.parent;
            Transform mainArea  = Area.parent;

            areaName = mainArea.name;
            QuestEvents.OnAreaEnable += ReturnObject;

            tilemap = tileLayer.GetComponent<Tilemap>();
            positionOnTileMap = new Vector3Int((int) (transform.localPosition.x - tilemap.tileAnchor.x), (int) (transform.localPosition.y - tilemap.tileAnchor.y), 0);
            tileBase = tilemap.GetTile(positionOnTileMap);
        }
        catch (System.Exception e)
        {            
            Debug.LogError(e + ", немозможно найти родителя с названием зоны для объекта, метод Breakable не будет активирован");
        }

        particleSys = transform.Find("Particles");
        if (particleSys != null)
        {
            particleSystemDamage = particleSys.GetComponent<ParticleSystem>();  
        }
        
    }
    public void DoBroke()
    {
        DoParticleSystemDamage();
        gameObject.SetActive(false);  
        
        tilemap.SetTile(positionOnTileMap, null);    
        
        /*foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {
                Debug.Log(place.ToString());
            }
        }*/
    }
    public void ReturnObject(string areaNameFromEventCall)
    {
        if (areaNameFromEventCall == areaName && gameObject.activeSelf == false)        
        {
            gameObject.SetActive(true); 
            if (tileBase != null)
                tilemap.SetTile(positionOnTileMap, tileBase);           
        }
    }

    public void DoParticleSystemDamage()//Vector2 impactPosition)
    {
        if (particleSystemDamage != null)
        {
            particleSys.SetParent(transform.parent);
            //float angle = MyMathCalculations.CalculateAngle2D(impactPosition, cam.ScreenToWorldPoint(Input.mousePosition), transform.right);
            //particleSys.localRotation = Quaternion.Euler(angle,90f,0f);

            //if(!particleSystemDamage.isPlaying) 
            particleSystemDamage.Play();
            Destroy(particleSys.gameObject, 1f);
        }
        else
            Debug.Log("have no particel syctem");
    }
}
