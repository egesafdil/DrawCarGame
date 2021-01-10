using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Direction : MonoBehaviour
{   
    [Range(-1, 1)] [SerializeField] int carDirection = 1;
    [SerializeField] bool elevator = false;
    [SerializeField] GameObject elevatorLevel;
    [SerializeField] CinemachineVirtualCamera camera;

    private bool carEntered = false;
    private float elevatorSpeed = 2f;
    private float timer = 0;
    private DrawCar drawCar;

    private void Start()
    {
        drawCar = FindObjectOfType<DrawCar>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Car")
        {
            drawCar.Direction = carDirection;
            carEntered = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            drawCar.Direction = carDirection;
            carEntered = false;
        }
    }

    private void Update()
    {
        if (elevator && carEntered)
        {
            if (timer >= 2)
            {
                carDirection = 0;
                camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(5.0f, 2.0f, 3.0f);
                float step = elevatorSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 10.8f), step);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        if (transform.position.y >= 10f && carEntered && elevator)
        {
            
            drawCar.Direction = -1;
            StartCoroutine(ElevatorGoBack());
        }
        else if(transform.position.y >= 4f && !carEntered && elevator){
            StartCoroutine(ElevatorGoBack());
        }
    }

    IEnumerator ElevatorGoBack()
    {
        yield return new WaitForSeconds(5f);
        timer = 0;
        transform.position = new Vector3(transform.position.x, 3.225f);
    }
}
