using UnityEngine;

public class SecondFloorTouching : MonoBehaviour
{   
    private void OnCollisionEnter(Collision other) {
        other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x,0f,other.gameObject.transform.position.z);    
    }
}
