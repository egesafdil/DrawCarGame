using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Car")
        {
            DrawCar drawCar = FindObjectOfType<DrawCar>();
            drawCar.InstantiateCheckpoint();
        }
    }
}
