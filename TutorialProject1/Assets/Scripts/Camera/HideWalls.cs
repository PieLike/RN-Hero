//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Wall" && other.gameObject.transform.localScale.y < 1)
        {
            /*MeshRenderer wallMesh = other.gameObject.GetComponent<MeshRenderer>();  
            if (wallMesh != null)
                if (wallMesh.enabled == true)
                    wallMesh.enabled = false;*/

            Animator wallAnim = other.gameObject.GetComponent<Animator>();  
            if (wallAnim != null)
                wallAnim.SetBool("fade", true);   
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Wall" && other.gameObject.transform.localScale.y < 1)
        {
            Animator wallAnim = other.gameObject.GetComponent<Animator>();  
            if (wallAnim != null)
                wallAnim.SetBool("fade", false);   
        }         
    }
}
