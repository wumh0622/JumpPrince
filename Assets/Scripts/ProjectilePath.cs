using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePath : MonoBehaviour
{
    public Vector2 initialVelocity;   // 初始速度
    public float timeResolution = 0.02f;   // 時間解析度
    public float maxTime = 10f;            // 最大模擬時間
    public LineRenderer lineRenderer;      // 線段渲染器
    bool drawing;
    float gravityModifier;

    private void Start()
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        gravityModifier = GetComponentInParent<PlayerController>().gravityModifier;
    }

    public void DrawPath(Vector2 velocity)
    {
        drawing = true;
        initialVelocity = velocity;
    }

    public void StopDrawing()
    {
        drawing = false;
        lineRenderer.enabled = false;
    }


    private void FixedUpdate()
    {
        if (drawing)
        {
            CalculatePath();
            lineRenderer.enabled = true;
        }
    }

    private void CalculatePath()
    {
        Vector2 currentPosition = transform.position;
        Vector2 currentVelocity = initialVelocity;

        int numPositions = Mathf.RoundToInt(maxTime / timeResolution) + 1;
        lineRenderer.positionCount = numPositions;
        lineRenderer.SetPosition(0, currentPosition);

        for (int i = 1; i < numPositions; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentVelocity, currentVelocity.magnitude * Time.deltaTime, 1);
            if (hit.collider is null)
            {
                currentVelocity += Physics2D.gravity * Time.deltaTime;
                currentPosition += currentVelocity * Time.deltaTime;

                lineRenderer.SetPosition(i, currentPosition);
            }
            else 
            {
                currentPosition += currentVelocity.normalized * hit.distance;
                lineRenderer.SetPosition(i, currentPosition);
                lineRenderer.positionCount = i + 1;
                break;
            }
        }
    }
}
