using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDash : MonoBehaviour
{
    private Rigidbody2D rigidBody;//  Camera cam;
    public float dashPower; public bool dashEnabled;
    private float timeDashDisable;
    HeroView heroView;
    private void Start() 
    {
        rigidBody = GetComponent<Rigidbody2D>();

        heroView = GetComponent<HeroView>();
        //cam = Camera.main;
    }
    private void Update() 
    {
        //if(dashEnabled && timeWhenDashDisabled != 0f && timeWhenDashDisabled <= Time.time)
        //    DisableDash();

        if (dashEnabled && Input.GetKeyDown(KeyCode.Space) && MainVariables.allowMovement)
        {
            DashToWASD();
        }

        if (dashEnabled)
        {
            if (Time.time > timeDashDisable) 
            {
                DisableDash();
            }
        }
    }
    private void DashToWASD()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            MainVariables.inMovement = false;
            rigidBody.velocity = Vector2.zero;
            rigidBody.Sleep();

            MainVariables.isDashing = true;
            MainVariables.allowMovement = false;

            /*Vector2 direction;
            if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
                direction = new Vector2(Input.GetAxisRaw("Horizontal") / 2, Input.GetAxisRaw("Vertical") / 2);
            else
                direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));*/

            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            rigidBody.velocity = direction * dashPower;

            StopCoroutine(MoveStopCoroutine(0.2f, 0.5f));
            StartCoroutine(MoveStopCoroutine(0.2f, 0.5f));
        }
    }
    /*private void DashToMousePosition()
    {
        MainVariables.inMovement = false;
        MainVariables.isDashing = true;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = (Vector2) (mousePos - gameObject.transform.position).normalized;

        Vector2 direction = MyMathCalculations.CalculateDirectionSpeeds(gameObject.transform.position, mousePos);

        Debug.Log(direction.ToString());
        rigidBody.velocity = direction * dashPower;

        StopCoroutine(MoveStopCoroutine(0.5f));
        StartCoroutine(MoveStopCoroutine(0.5f));
    }*/
    private IEnumerator MoveStopCoroutine(float moveDuration, float dashDuration)
    {
        heroView.BeginReload(Time.time, Time.time + dashDuration);
        dashEnabled = false;  
    
        yield return new WaitForSeconds(moveDuration);
        MainVariables.isDashing = false; 

        yield return new WaitForSeconds(dashDuration);
        dashEnabled = true;  
    }

    public void EnableDash(float duration)
    {
        dashEnabled = true;
        timeDashDisable = timeDashDisable == 0f ? Time.time + duration : timeDashDisable + duration;
    }
    public void DisableDash()
    {
        dashEnabled = false;
        timeDashDisable = 0f;
    }
    /*private IEnumerator DashDisableCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        dashEnabled = false;  
    }*/
}
