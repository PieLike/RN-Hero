using UnityEngine;
//using System.Collections;

public class FollowCamera : MonoBehaviour 
{

    private GameObject target;
    private Vector3 offset; 
    [SerializeField] private float smooth = 100f; 

    private float boardUp, boardDown, boardRight, boardLeft;
    private float cameraY, cameraX;
    private GameObject cameraBoards;

    void Start () 
    {       
        //находим объекты на сцене
        target = GameObject.Find("Hero");
        cameraBoards = GameObject.Find("CameraBoards");

        offset = transform.position - target.transform.position;

        CalculateBorders();
    }    

    void  Update ()
    {
        if (target.transform.position.z > boardUp)
            cameraY = boardUp;
        else
        {
            if (target.transform.position.z < boardDown)
                cameraY = boardDown;
            else
                cameraY = target.transform.position.z; 
        } 

        if (target.transform.position.x > boardRight)
            cameraX = boardRight;
        else
        {
            if (target.transform.position.x < boardLeft)
                cameraX = boardLeft;
            else
                cameraX = target.transform.position.x; 
        }         

        transform.position = Vector3.Lerp (transform.position, new Vector3(cameraX, target.transform.position.y, cameraY) + offset, Time.deltaTime * smooth);
        transform.LookAt(target.transform);
    } 

    public void CalculateBorders()
    {
        Transform lane = cameraBoards.transform;
        boardUp = lane.position.z + lane.localScale.z * 5f;
        boardDown = lane.position.z - lane.localScale.z * 5f;
        boardRight = lane.position.x + lane.localScale.x * 5f;
        boardLeft = lane.position.x - lane.localScale.x * 5f;
    }
}