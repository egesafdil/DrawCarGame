using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracking : MonoBehaviour
{
    [SerializeField] Camera drawCam;

    private LineRenderer lineRenderer;
    private List<Vector3> linePositions;
    
    void Start()
    {
        linePositions = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = drawCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!linePositions.Contains(hit.point))
                {
                    linePositions.Add(hit.point);
                }

            }
            DrawScreen();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            DrawCar drawCar = FindObjectOfType<DrawCar>();
            drawCar.Draw(linePositions);
            linePositions.Clear();

        }
    }

    void DrawScreen()
    {
        lineRenderer.positionCount = linePositions.Count;
        lineRenderer.SetPositions(linePositions.ToArray());
    }

}
