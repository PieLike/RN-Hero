using UnityEngine;

public class AreaChanger : MonoBehaviour
{
    private string prevAreaName = "";
    public static string areaName;
    public GameObject[] activeAreas;
    private GameObject cameraBoards;
    private FollowCamera mainCameraFollow;

    private void Start() 
    {
        cameraBoards = GameObject.Find("CameraBoards");

        //Camera mainCamera = Camera.main;
        mainCameraFollow = Camera.main.GetComponent<FollowCamera>();

        activeAreas = new GameObject[9];    
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
                    activeAreas[1] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны снизу
            if(row != '0')
            {
                newRow = (char)(((int)row) - 1);
                newArea = string.Concat(column, newRow);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[2] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны справа
            if(column != 'Z')
            {
                newColumn = (char)(((int)column) + 1);
                newArea = string.Concat(newColumn, row);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[3] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны слева
            if(column != 'A')
            {
                newColumn = (char)(((int)column) - 1);
                newArea = string.Concat(newColumn, row);
                NewActiveAreaParent = GameObject.Find(newArea);
                if (NewActiveAreaParent != null)
                    activeAreas[4] = NewActiveAreaParent.transform.Find("Area").gameObject;
            }
            //находим зоны сверху слева
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

            ChangeCameraBoards();   //изменить грани движения камеры под новую зону

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
        cameraBoards.transform.position = activeAreas[0].transform.position; //cameraBoards.transform.position +
        cameraBoards.transform.localScale = new Vector3(3.2f, cameraBoards.transform.localScale.y, 3.2f); 

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
        
        
        mainCameraFollow.CalculateBorders();
    } 

    private void AreaReachRegistration()
    {
        //зарегестрировать нахождение в новой зоне в ивент менеджере
        QuestEvents.SendReachArea(areaName);
    }
}
