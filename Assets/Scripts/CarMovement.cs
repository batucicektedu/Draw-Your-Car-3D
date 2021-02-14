using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float carAccelerationConstant = 5f;

    private Rigidbody thisRigidbody;

    // ************> TODO: change this to false when start and end game events are in use <************ //
    private bool canMove = true;
    private float durationInMotion = 0;
    private Camera cam;

    private void Start()
    {
        GameManager.Instance.GameStarted += StartMoving;
        thisRigidbody = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (canMove)
        {
            /*Count the seconds car is in motion
            Since it also affects accelation it is capped at 5*/
            if(durationInMotion < 5f) {
                durationInMotion += Time.deltaTime;
            }
            
            //Power of 2 is used in order to get a graph that is slowly accelerating in the beginning then gets really fast near the cap
            thisRigidbody.velocity = new Vector3(carAccelerationConstant * Mathf.Pow(durationInMotion, 2), thisRigidbody.velocity.y, thisRigidbody.velocity.z);

            //Debug.Log("car velocity :  " + thisRigidbody.velocity);
        }

        cam.fieldOfView = 60 + durationInMotion * 5;
    }

    private void FixedUpdate()
    {
        canMove = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            canMove = false;
            durationInMotion = 0;
        }
    }

    private void StartMoving()
    {
        ResetMovement();
    }

    private void ResetMovement()
    {
        canMove = true;
        durationInMotion = 0;
    }


    private void OnDisable()
    {
        GameManager.Instance.GameStarted -= StartMoving;
    }
}
