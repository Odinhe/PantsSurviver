using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    Transform targetToLook;
    // Use this for initialization
    void Start()
    {
        //set where the game object will look at
        targetToLook = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //get the game game object to the angle that they should be looking, which is the camera
        transform.rotation = Quaternion.LookRotation(targetToLook.forward, Vector3.up);
    }
}

