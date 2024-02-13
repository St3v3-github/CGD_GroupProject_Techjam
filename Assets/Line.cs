using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Transform startPoint; // Reference to the start point GameObject
    public Transform endPoint; // Reference to the end point GameObject

    private LineRenderer lineRenderer;

    void Start()
    {
        // Get or add Line Renderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set Line Renderer properties
        lineRenderer.positionCount = 2; // Two points (start and end)
        lineRenderer.startWidth = 0.1f; // Set start width
        lineRenderer.endWidth = 0.1f; // Set end width

        // Set initial positions
        UpdateLinePositions();
    }

    void Update()
    {
        // Update positions every frame (optional)
        UpdateLinePositions();
    }

    void UpdateLinePositions()
    {
        // Set positions of start and end points
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
    public void setPoints(Transform startPoint, Transform endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }
}
