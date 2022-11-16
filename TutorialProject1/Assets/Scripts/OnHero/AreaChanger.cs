using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System;

public class AreaChanger : MonoBehaviour
{
    public enum AreaType {main, right, left, up, down, other}
    private string prevAreaName = "";
    public static string areaName;    
    [SerializeField] public List<Area> activeAreas;
    private GameObject cameraBoards;
    private FollowCamera mainCameraFollow;
    //bool readyToLeave; 
    GameObject potentialArea;
    LayerMask wallsLayer;
    Transform worldTransform;

    public class Area
    {
        public string name = "";
        public GameObject gameObj;
        public GridGraph graphAI;
        public AreaType type;
        public SpawnChecker spawn;
    }

    private void Start() 
    {
        cameraBoards = GameObject.Find("CameraBoards");
        mainCameraFollow = Camera.main.GetComponent<FollowCamera>();

        worldTransform = GameObject.Find("World").transform;

        activeAreas = new List<Area>();
            activeAreas.Add(new Area { type = AreaType.main }); 
            activeAreas.Add(new Area { type = AreaType.right });
            activeAreas.Add(new Area { type = AreaType.left });
            activeAreas.Add(new Area { type = AreaType.up });
            activeAreas.Add(new Area { type = AreaType.down });

        wallsLayer = LayerMask.GetMask("Walls");

        //DeactivateAreas();
    }
    /*private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Area")
        {
            potentialArea = other.gameObject; 
            readyToLeave = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Area" && readyToLeave && potentialArea != null)
        {
            //получаем имя parent 
            areaName = potentialArea.name; 
            activeAreas[0] = potentialArea.transform.Find("Area").gameObject;

            readyToLeave = false; 
        }     
    }*/
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Area")// && gameObject.tag == "Hero")
        {
            potentialArea = other.gameObject;
            if(areaName != potentialArea.name)
            {                
                //получаем имя parent       
                areaName = potentialArea.name; 
                //readyToLeave = false;                  
            }  
        }        
    }

    private void SetActiveForActiveAreas()
    {
        //деактивруем старые зоны
        DeactivateAreas();
        //если зона не равна null и неактивна, то активируем
        foreach(Area area in activeAreas)
        {
            if(area.gameObj != null)
            {
                if(area.gameObj.activeSelf == false)
                {
                    area.gameObj.SetActive(true);
                    QuestEvents.SendInteractionNpc(area.name);
                }
            }
        }
    }
    private void DeactivateAreas()
    {
        /*foreach(Area activeArea in activeAreas)
        {
            Debug.Log(activeArea.name);
        }  */  
        //деактивруем старые зоны
        GameObject[] areas;
        areas = GameObject.FindGameObjectsWithTag("SubArea");
        foreach(GameObject area in areas)
        {
            if(area.activeSelf == true)
            {
                //Debug.Log(area.transform.parent.name);
                bool isInNewAreasList = CompairAreas(area);
                if (isInNewAreasList == false)
                    area.SetActive(false);
            }    
        }
    }
    private bool CompairAreas(GameObject area)
    {
        //если зона не совпадает с зоной будущих активных зон то деактивируем ее
        foreach(Area activeArea in activeAreas)
        {
            //Debug.Log(activeArea.name);

            if(activeArea != null && activeArea.name != "")
            {
                if(area.transform.parent.name == activeArea.name)
                {
                    return true;
                    //area.SetActive(false);
                }                             
            }
        }
        return false;
    }

    /*private void FixedUpdate() 
    {
        string area = "";
        char row = (char) 1;//((int) Math.Round(transform.position.x / 16) + 10);
        area = string.Concat(area, row);
        int yPosition = (int) (Math.Round(transform.position.y / 16));
        Debug.Log(area);
    }*/

    private void Update() 
    {
        if (areaName == null) return;

        
        if (prevAreaName != areaName)
        //при изменении сцены
        {
            //Debug.Log(prevAreaName + " = " + areaName);

            AreaReachRegistration();

            //ClearAreaArray();

            AreaType directionOfNewArea =  FindAreasRelationship(prevAreaName, areaName);
            if (directionOfNewArea == AreaType.other)
                ConstructBrandNew(areaName);
            else if (directionOfNewArea == AreaType.main)   
                return;
            else
                Construct(directionOfNewArea, areaName);

            prevAreaName = areaName;             

            //деактивируем старые зоны и активруем новые
            SetActiveForActiveAreas();   

            AstarPath.active.Scan();// Debug.Log("scan");
            //gameObject.SendMessage("Scan",SendMessageOptions.DontRequireReceiver);  //scan the map   

            ChangeCameraBoards();   //изменить грани движения камеры под новую зону 

            UpdateEnemies();    
        }  
    }

    private void UpdateEnemies()
    {
        foreach (Area area in activeAreas)
        {
            if (area.spawn == null)
            {
                Transform spawnPoints = area.gameObj.transform.Find("Grid/spawnPoints");
                if (spawnPoints != null)
                    area.spawn = spawnPoints.GetComponent<SpawnChecker>();
                else
                    Debug.LogError("cant find Area/Grid/spawnPoints in " + area.name);
            }

            if (area.spawn != null)
            {
                if (area.type == AreaType.main)
                    area.spawn.StartAllSpawnEnemies();
                else
                    area.spawn.StopAllSpawnEnemies();
            }    
        }
    }

    private void ConstructBrandNew(string mainAreaName)
    {
        RemoveAllAreas();
        AddAreaMain(mainAreaName);
        AddArea(AreaType.up);
        AddArea(AreaType.down);
        AddArea(AreaType.right);
        AddArea(AreaType.left);        
    }
    private void Construct(AreaType type, string mainAreaName)	//up
    {
        RemoveArea(GetOppositeSide(type));	//down

        GetActiveArea(AreaType.main).type = GetOppositeSide(type);	//main 	-> down
        Area newMain = GetActiveArea(type);
        if (newMain == null)
        {
            ConstructBrandNew(areaName);
            return;
        }
        newMain.type = AreaType.main;			//up 	-> main

        AddArea(type);	//up	
        
        AddAndRemoveSides(type);
    }

    private void RemoveArea(AreaType type)
    {
        Area area = GetActiveArea(type);
        if (area != null)
        {
            RemoveGridGraph(area.graphAI);
            activeAreas.Remove(area);
        }
    }
    private void RemoveAllAreas()
    {
        //foreach (Area area in activeAreas)
        foreach (GridGraph graphAI in AstarPath.active.data.GetUpdateableGraphs())
        {
            //RemoveGridGraph(area.graphAI);
            RemoveGridGraph(graphAI);
        }       
        activeAreas.Clear();
    }

    private void AddArea(AreaType type)
    {
        Area area = new Area();
        area.type = type;

        Area mainArea = GetActiveArea(AreaType.main);
        area.gameObj = FindAreaObj(mainArea.gameObj, type, mainArea.name);  
        if (area.gameObj == null) return;      

        area.name = area.gameObj.transform.parent.name;   
        area.graphAI = AddGridGraph(area.gameObj.transform.position, area.name + " " + type.ToString());

        activeAreas.Add(area); // Debug.Log("added area " + area.type.ToString());
    }
    private void AddAreaMain(string _areaName)
    {
        Area area = new Area();
        area.name = _areaName;
        area.type = AreaType.main;

        Transform FoundAreaParent = worldTransform.Find($"Row{_areaName[0]}/"+ _areaName);
        if (FoundAreaParent != null)
            area.gameObj = FoundAreaParent.Find("Area").gameObject;
        else
            Debug.LogError("cant find: " + _areaName);

        area.graphAI = AddGridGraph(area.gameObj.transform.position, _areaName + " main");

        activeAreas.Add(area);
    }

    private GameObject FindAreaObj(GameObject mainArea, AreaType type, string mainAreaName)
    {
        //string mainAreaName = mainArea.name;
        Transform FoundAreaParent;

        char column = mainAreaName[0];
        char row = mainAreaName[1];
        
        char newColumn;
        char newRow;
        string newArea;

        switch (type)
        {
            case AreaType.up:
                if(row != '9')
                {
                    newRow = (char)(((int)row) + 1);
                    newArea = string.Concat(column, newRow);
                    FoundAreaParent = worldTransform.Find($"Row{column}/"+ newArea);
                    if (FoundAreaParent != null)
                        return FoundAreaParent.Find("Area").gameObject;
                }
                break;
            case AreaType.down:
                if(row != '0')
                {
                    newRow = (char)(((int)row) - 1);
                    newArea = string.Concat(column, newRow);
                    FoundAreaParent = worldTransform.Find($"Row{column}/"+ newArea);
                    if (FoundAreaParent != null)
                        return FoundAreaParent.Find("Area").gameObject;
                }
                break;
            case AreaType.right:
                if(column != 'Z')
                {
                    newColumn = (char)(((int)column) + 1);
                    newArea = string.Concat(newColumn, row);
                    FoundAreaParent = worldTransform.Find($"Row{newColumn}/"+ newArea);
                    if (FoundAreaParent != null)
                        return FoundAreaParent.Find("Area").gameObject;
                }
                break;
            case AreaType.left:
                if(column != 'A')
                {
                    newColumn = (char)(((int)column) - 1);
                    newArea = string.Concat(newColumn, row);
                    FoundAreaParent = worldTransform.Find($"Row{newColumn}/"+ newArea);
                    if (FoundAreaParent != null)
                        return FoundAreaParent.Find("Area").gameObject;
                }
                break;            
        }
        return null;
    }

    private AreaType GetOppositeSide(AreaType type)
    {
        switch(type)
        {
            case AreaType.up: return AreaType.down;
            case AreaType.down : return AreaType.up;
            case AreaType.right: return AreaType.left;
            case AreaType.left : return AreaType.right;
        }
        return AreaType.main;
    }

    private void AddAndRemoveSides(AreaType type)
    {
        if (type == AreaType.up || type == AreaType.down)
        {
            RemoveArea(AreaType.right);
            AddArea(AreaType.right);
            RemoveArea(AreaType.left); 
            AddArea(AreaType.left);
        }
        else if (type == AreaType.right || type == AreaType.left) 
        {
            RemoveArea(AreaType.up); 
            AddArea(AreaType.up);
            RemoveArea(AreaType.down); 
            AddArea(AreaType.down);
        }
    }

    private GridGraph AddGridGraph(Vector3 position, string areaName)
    {
        //Vector3 position = areaObject.transform.position;

        int width = 50, depth = 28;
        float nodeSize = 0.5f;

        var newGraph = (GridGraph) AstarPath.active.data.AddGraph(typeof(GridGraph));     
        newGraph.name = areaName;

        newGraph.SetDimensions(width, depth, nodeSize);
        newGraph.RelocateNodes(position, Quaternion.identity, nodeSize);
        newGraph.is2D = true;

        newGraph.collision.use2D = true;
        newGraph.collision.type = ColliderType.Sphere;
        newGraph.collision.diameter = 1.3f;
        newGraph.collision.mask = wallsLayer;

        newGraph.cutCorners = false;

        //CalculateAffectedRegions()
        return newGraph;
    }

    private void RemoveGridGraph(GridGraph graph)
    {
        if (graph != null)
            AstarPath.active.data.RemoveGraph(graph);
    }

    /*private void UpdateSpawnEnemies(GameObject spawnPointsObj)
    {
        //List<GameObject> spawnPoints = new List<GameObject>();
        foreach (Transform child in spawnPointsObj.transform)
        {
            //spawnPoints.Add
            SpawnPoint spawnPoint = child.GetComponent<SpawnPoint>();
            if (spawnPoint != null)
            {
                //spawnPoint.ReDoSpawn();
                //spawnPoint.DestroySpawnedEnemies();
            }            
        }
    }*/

    /*private void ClearAreaArray()
    {
        //оцищаем массив кроме первого элемента, потому что это текущая зона
        for(int i = 1; i < activeAreas.Count; i++)
        {
            if(activeAreas[i].gameObj != null)
            {
                activeAreas[i].gameObj = null;
            }
        }   
    }*/

    private void ChangeCameraBoards()
    //изменить грани движения камеры под новую зону
    {
        cameraBoards.transform.position = GetActiveArea(AreaType.main).gameObj.transform.position;
        //Debug.Log(GetActiveArea(AreaType.main).gameObj.transform.position.ToString());
        //activeAreas[0].gameObj.transform.position; //cameraBoards.transform.position +
        /*cameraBoards.transform.localScale = new Vector3(3.2f, cameraBoards.transform.localScale.y, 3.2f); 

        if(activeAreas[2] == null)// если снизу НЕТ зоны, тотянем до туда борд
        {
            float down_offset = 9f; 
            cameraBoards.transform.localScale = new Vector3(cameraBoards.transform.localScale.x, cameraBoards.transform.localScale.y, cameraBoards.transform.localScale.z - down_offset*0.1f);
            cameraBoards.transform.position = new Vector3(cameraBoards.transform.position.x, cameraBoards.transform.position.y, cameraBoards.transform.position.z + down_offset*0.5f);
        }
        if(activeAreas[3] == null)// если справа НЕТ зоны, тотянем до туда борд
        {
            float right_offset = 8f; 
            cameraBoards.transform.localScale = new Vector3(cameraBoards.transform.localScale.x - right_offset*0.1f, cameraBoards.transform.localScale.y, cameraBoards.transform.localScale.z);
            cameraBoards.transform.position = new Vector3(cameraBoards.transform.position.x - right_offset*0.5f, cameraBoards.transform.position.y, cameraBoards.transform.position.z);
        }
        if(activeAreas[4] == null)// если слева НЕТ зоны, тотянем до туда борд
        {
            float left_offset = 8f; 
            cameraBoards.transform.localScale = new Vector3(cameraBoards.transform.localScale.x - left_offset*0.1f, cameraBoards.transform.localScale.y, cameraBoards.transform.localScale.z);
            //if(activeAreas[3] == null)
                cameraBoards.transform.position = new Vector3(cameraBoards.transform.position.x + left_offset*0.5f, cameraBoards.transform.position.y, cameraBoards.transform.position.z);
            //else
            //    cameraBoards.transform.position = new Vector3(cameraBoards.transform.position.x - left_offset*0.5f, cameraBoards.transform.position.y, cameraBoards.transform.position.z);
        }
        */
        
        mainCameraFollow.CalculateBorders();
    } 

    private void AreaReachRegistration()
    {
        //зарегестрировать нахождение в новой зоне в ивент менеджере
        QuestEvents.SendReachArea(areaName);
    }

    private Area GetActiveArea(AreaType type)
    {
        foreach (var item in activeAreas)
        {
            if (item.type == type)
                return item;
        }
        return null;
    }

    private AreaType FindAreasRelationship(string prevArea, string newArea)
    {
        if (prevArea == "")
            return AreaType.other;

        char prevColumn = prevArea[0], prevRow = prevArea[1];
        char newColumn = newArea[0], newRow = newArea[1];

        if (prevColumn == newColumn)    //одинаковая (буква, колонка)
        {
            if (prevRow == newRow)      //оба одинаковы, нет изменений
                return AreaType.main;
            
            if( newRow > prevRow )
                return AreaType.up;
            else
                return AreaType.down;

        }else if (prevRow == newRow)    //одинаковая (цифра, строка)
        {
            if( (int)newColumn > (int)prevColumn )  //у новой колонки следующая буква, то есть вправо
             return AreaType.right;
            else
                return AreaType.left;
                
        }
        else                            //и то и другое отличается
        {
            return AreaType.other;
        }
    }

    /*private void OldUpdate() 
    {
        if (areaName == null) return;

        if (prevAreaName != areaName)
        //при изменении сцены
        {
            AreaReachRegistration();

            ClearAreaArray();

            char column = areaName[0];
            char row = areaName[1];
            
            char newColumn;
            char newRow;
            string newArea;

            GameObject NewActiveAreaParent;

            //находим зоны сверху
            if(row != '9')
            {
                newRow = (char)(((int)row) + 1);
                newArea = string.Concat(column, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    GetActiveArea(AreaType.up).gameObj = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны снизу
            if(row != '0')
            {
                newRow = (char)(((int)row) - 1);
                newArea = string.Concat(column, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    GetActiveArea(AreaType.down).gameObj = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны справа
            if(column != 'Z')
            {
                newColumn = (char)(((int)column) + 1);
                newArea = string.Concat(newColumn, row);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    GetActiveArea(AreaType.right).gameObj = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны слева
            if(column != 'A')
            {
                newColumn = (char)(((int)column) - 1);
                newArea = string.Concat(newColumn, row);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    GetActiveArea(AreaType.left).gameObj = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
        /*    //находим зоны сверху слева
            if(row != '9' && (column != 'A'))
            {
                newColumn = (char)(((int)column) - 1);
                newRow = (char)(((int)row) + 1);
                newArea = string.Concat(newColumn, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[5] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны сверху справа
            if(row != '9' && (column != 'Z'))
            {
                newColumn = (char)(((int)column) + 1);
                newRow = (char)(((int)row) + 1);
                newArea = string.Concat(newColumn, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[6] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны снизу слева
            if(row != '0' && (column != 'A'))
            {
                newColumn = (char)(((int)column) - 1);
                newRow = (char)(((int)row) - 1);
                newArea = string.Concat(newColumn, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[7] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны снизу справа
            if(row != '0' && (column != 'Z'))
            {
                newColumn = (char)(((int)column) + 1);
                newRow = (char)(((int)row) - 1);
                newArea = string.Concat(newColumn, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[8] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }

            prevAreaName = areaName;

            //UpdateGridGraphs();            
            gameObject.SendMessage("Scan",SendMessageOptions.DontRequireReceiver);  //scan the map

            ChangeCameraBoards();   //изменить грани движения камеры под новую зону

            //деактивируем старые зоны и активруем новые
            SetActiveForActiveAreas();       

            //UpdateSpawnEnemies();    
        }  
    }*/
}
