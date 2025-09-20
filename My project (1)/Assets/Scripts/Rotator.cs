using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float speedX; //Rotate by this amount per frame
    [SerializeField] float speedY; //Rotate by this amount per frame
    [SerializeField] float speedZ; //Rotate by this amount per frame

    // Update is called once per frame
    void Update()
    {
        //Rotate the transform by the above variables.
        transform.Rotate(speedX, speedY, speedZ);
    }
}

