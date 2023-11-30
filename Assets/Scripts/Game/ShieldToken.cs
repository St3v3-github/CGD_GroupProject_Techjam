using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldToken : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float hoverHeight = 0.1f;
    public float hoverSpeed = 0.5f;
    public GameObject Team1Mesh;
    public GameObject Team2Mesh;
    private int team;

    private void Update()
    {
        // Rotate the object
        RotateObject();

        // Hover the object up and down
        HoverObject();
    }

    public void setTeam(int _team)
    {
        team = _team;
        if (team == 1)
        {
            Team1Mesh.SetActive(true);
            Team1Mesh.SetActive(false);
        }
        else if (team == 2)
        {
            Team1Mesh.SetActive(false);
            Team1Mesh.SetActive(true);
        }
    }

    private void RotateObject()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void HoverObject()
    {
        // Calculate the new Y position for hovering
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Apply the new position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
