using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlane : MonoBehaviour
{
    [SerializeField] Vector3 jumpForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            DrawCar drawCar = FindObjectOfType<DrawCar>();
            drawCar.Jump(jumpForce);
           
        }
    }
}
