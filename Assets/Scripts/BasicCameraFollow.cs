using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraFollow : MonoBehaviour
{
    public Transform car;

    private float carLastPositionX;

    void Start()
    {
        //Store the initial X position of the car
        carLastPositionX = car.position.x;
    }


    void Update()
    {
        //Follow the car on X axis by applying the change in car's position to the camera's 

        float playerPositionChangeOnThisFrame = car.position.x - carLastPositionX;
        carLastPositionX = car.position.x;

        transform.position += new Vector3(playerPositionChangeOnThisFrame, 0, 0);

    }
}
