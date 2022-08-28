using UnityEngine;

public class AreaChanger : MonoBehaviour
{
    private string prevAreaName = "";
    public static string areaName;
    private GameObject[] activeAreas;
    private GameObject cameraBoards;
    private FollowCamera mainCameraFollow;

    private void Start() 
    {
        cameraBoards = GameObject.Find("CameraBoards");

        //Camera mainCamera = Camera.main;
        mainCameraFollow = Camera.main.GetComponent<FollowCamera>();

        activeAreas = new GameObject[5];    
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Area")
        {
            //получаем имя parent 
            areaName = other.gameObject.name; 
            activeAreas[0] = other.gameObject.transform.Find("Area").gameObject; 
        }    
    }

    private void SetActiveForActiveAreas()
    {
        //деактивруем старые зоны
        DeactivateAreas();
        //если зона не равна null и неактивна, то активируем
        foreach(GameObject area in activeAreas)
        {
            if(area != null)
            {
                if(area.activeSelf == false)
                    area.SetActive(true);
            }
        }
    }
    private void DeactivateAreas()
    {
        //деактивруем старые зоны
        GameObject[] areas;
        areas = GameObject.FindGameObjectsWithTag("SubArea");
        foreach(GameObject area in areas)
        {
            if(area.activeSelf == true)
            {
                //если зона не совпадает с зоной будущих активных зон то деактивируем ее
                foreach(GameObject actualArea in activeAreas)
                {
                    if(actualArea != null)
                    {
                        if(area != actualArea)
                        {
                            area.SetActive(false);
                        }                             
                    }
                }
            }    
        }
    }

    private void Update() 
    {
        if (prevAreaName != areaName)
        //при изменении сцены
        {
            ChangeCameraBoards();   //изменить грани движения камеры под новую зону

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
                    activeAreas[1] = NewActiveAreaParent.transform.Find("Area").gameObject;
                //Debug.Log(newArea);
            }
            //находим зоны снизу
            if(row != '0')
            {
                newRow = (char)(((int)row) - 1);
                newArea = string.Concat(column, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[2] = NewActiveAreaParent.transform.Find("Area").gameObject;
                //Debug.Log(newArea);
            }
            //находим зоны справа
            if(column != 'Z')
            {
                newColumn = (char)(((int)column) + 1);
                newArea = string.Concat(newColumn, row);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[3] = NewActiveAreaParent.transform.Find("Area").gameObject;
                //Debug.Log(newArea);
            }
            //находим зоны слева
            if(column != 'A')
            {
                newColumn = (char)(((int)column) - 1);
                newArea = string.Concat(newColumn, row);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[4] = NewActiveAreaParent.transform.Find("Area").gameObject;
                //Debug.Log(newArea);
            }

            prevAreaName = areaName;
            //деактивируем старые зоны и активруем новые
            SetActiveForActiveAreas();            
        }  
    }

    private void ClearAreaArray()
    {
        //оцищаем массив кроме первого элемента, потому что это текущая зона
        for(int i = 1; i < activeAreas.Length; i++)
        {
            if(activeAreas[i] != null)
            {
                activeAreas[i] = null;
            }
        }   
    }

    private void ChangeCameraBoards()
    //изменить грани движения камеры под новую зону
    {
        cameraBoards.transform.position = cameraBoards.transform.position + activeAreas[0].transform.position;
        mainCameraFollow.CalculateBorders();
    } 
}
