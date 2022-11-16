using UnityEngine;
//using System.Collections;

public class FollowCamera : MonoBehaviour 
{

    private GameObject target;
    private Vector2 offset; 
    [SerializeField] private float smooth = 5f; 

    private float boardUp, boardDown;//, boardRight, boardLeft;
    private float cameraY;// cameraX;
    private GameObject cameraBoards;

    void Start () 
    {       
        //находим объекты на сцене
        target = GameObject.Find("Hero");
        cameraBoards = GameObject.Find("CameraBoards");

        offset = transform.position - target.transform.position;

        CalculateBorders();
        //transform.LookAt(target.transform);
        
    }    

    void  Update ()
    {
        if (target.transform.position.y > boardUp)
            cameraY = boardUp;
        else
        {
            if (target.transform.position.y < boardDown)
                cameraY = boardDown;
            else
                cameraY = target.transform.position.y; 
        } 

        /*if (target.transform.position.x > boardRight)
            cameraX = boardRight;
        else
        {
            if (target.transform.position.x < boardLeft)
                cameraX = boardLeft;
            else
                cameraX = target.transform.position.x; 
        }*/         

        transform.position = Vector3.Lerp (transform.position, new Vector3(cameraBoards.transform.position.x, cameraBoards.transform.position.y, transform.position.z), Time.deltaTime * smooth);
        //transform.position = Vector3.Lerp (transform.position, new Vector3(cameraBoards.transform.position.x, cameraY, transform.position.z), Time.deltaTime * smooth);

        //transform.position = Vector3.Lerp (transform.position, new Vector3(transform.position.x, target.transform.position.y, cameraY) + offset, Time.deltaTime * smooth);
        //transform.LookAt(target.transform);
    } 

    public void CalculateBorders()
    {
        Transform lane = cameraBoards.transform;
        //boardUp = lane.position.z + lane.localScale.z * 5f;
        //boardDown = lane.position.z - lane.localScale.z * 5f;
        //boardRight = lane.position.x + lane.localScale.x * 5f;
        //boardLeft = lane.position.x - lane.localScale.x * 5f;

        boardUp = lane.position.y + lane.localScale.y * 5f /10f;
        boardDown = lane.position.y - lane.localScale.y * 5f /10f;
    }
}