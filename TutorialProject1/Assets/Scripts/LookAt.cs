using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    public Transform GameObject;

    void Update()
    {
        transform.LookAt(GameObject);
        
    }
}
