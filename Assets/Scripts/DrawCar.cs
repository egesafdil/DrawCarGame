using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DrawCar : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] GameObject wheelPrefab;
    [SerializeField] LineRenderer lineRendererPrefab;
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] float movementSpeed = 7f;
    #endregion

    #region Caches
    private LineRenderer lineRenderer;
    private GameObject wheelLeft;
    private GameObject wheelRight;
    private Vector3 previousPosition;
    private GameObject checkPoint;
    private float timer = 0f;
    private int direction = 1;
    #endregion

    //Getter
    public int Direction
    {
        set { direction = value;
            if(value == 1)
            {
                camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(5.0f, 2.0f, -3.0f);
            }
            else if(value == -1)
            {
                camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(5.0f, 2.0f, 3.0f);
            }
        }
    }

    #region Start and Update
    void Start()
    {
        previousPosition = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        if (wheelLeft != null)
        {
            previousPosition = wheelLeft.transform.position;
            MoveCar();
            Timer();
        }
        
    }
    #endregion

    #region DrawingCar
    public void Draw(List<Vector3> linePositions)
    {
        Destroy(wheelLeft);
        Destroy(wheelRight);
        if(lineRenderer != null)
        {
            Destroy(lineRenderer.gameObject);
        }       

        float distanceX = previousPosition.x - linePositions[0].x;
        float distanceY = previousPosition.y - linePositions[0].y;

        float smallestValue = 100f;

        foreach(Vector3 position in linePositions)
        {
            if(position.y < smallestValue)
            {
                smallestValue = position.y;
            }
        }
        smallestValue = linePositions[0].y - smallestValue;

        List<Vector3> newLinePositions = new List<Vector3>();
        for (int i = 0; i < linePositions.Count-1; i+=3)
        {
            Vector3 newPosition = new Vector3(linePositions[i].x + distanceX, linePositions[i].y + distanceY+ smallestValue);
            newLinePositions.Add(newPosition);
        }
        InstantiateThings(newLinePositions);
        AddCollider(newLinePositions);
    }

    void InstantiateThings(List<Vector3> newLinePositions)
    {
        lineRenderer = Instantiate(lineRendererPrefab, new Vector3(0f,0f,0f), Quaternion.identity, gameObject.transform);
        lineRenderer.positionCount = newLinePositions.Count;
        lineRenderer.SetPositions(newLinePositions.ToArray());

        wheelLeft = Instantiate(wheelPrefab, newLinePositions[0], Quaternion.Euler(new Vector3(0f, 90f, 0f)), gameObject.transform);
        wheelLeft.GetComponent<FixedJoint>().connectedBody = lineRenderer.GetComponent<Rigidbody>();

        wheelRight = Instantiate(wheelPrefab, newLinePositions[newLinePositions.Count - 1], Quaternion.Euler(new Vector3(0f, 90f, 0f)), gameObject.transform);
        wheelRight.GetComponent<FixedJoint>().connectedBody = lineRenderer.GetComponent<Rigidbody>();


        camera.Follow = wheelLeft.transform;
        camera.LookAt = wheelLeft.transform;
    }

    void AddCollider(List<Vector3> newLinePositions)
    {
        for (int i = 3; i < newLinePositions.Count; i++ )
        {
            BoxCollider collider = lineRenderer.gameObject.AddComponent<BoxCollider>();
            collider.center = newLinePositions[i];
            collider.size = new Vector3(0.1f, 0.1f, 0.08f);
        }
    }
    #endregion

    #region Car Movement
    void MoveCar()
    {
        if (wheelLeft != null && wheelRight != null)
        {
            wheelLeft.GetComponent<Rigidbody>().AddForce(new Vector3(movementSpeed * direction * 100, 0f) * Time.deltaTime);
            wheelRight.GetComponent<Rigidbody>().AddForce(new Vector3(movementSpeed * direction * 100, 0f) * Time.deltaTime);
        }
    }

    public void Jump(Vector3 jumpVector)
    {
        wheelLeft.GetComponent<Rigidbody>().AddForce(jumpVector);
        wheelRight.GetComponent<Rigidbody>().AddForce(jumpVector);
    }

    #endregion

    #region Checkpoint
    public void SaveToCheckPoint(GameObject checkpointPosition)
    {
        checkPoint = checkpointPosition;
    }

    public void InstantiateCheckpoint()
    {
        Destroy(wheelLeft);
        Destroy(wheelRight);
        Destroy(lineRenderer.gameObject);
        previousPosition = checkPoint.transform.position;
        camera.Follow = checkPoint.transform;
        camera.LookAt = checkPoint.transform;
    }
    #endregion

    //Timer in case of not moving for a while
    void Timer()
    {
        if(Mathf.Abs(wheelLeft.GetComponent<Rigidbody>().velocity.x) <= 0.5f || Mathf.Abs(wheelRight.GetComponent<Rigidbody>().velocity.x) <= 0.5f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }

        if(timer >= 7f)
        {
            InstantiateCheckpoint();
            timer = 0f;
        }
    }

}
