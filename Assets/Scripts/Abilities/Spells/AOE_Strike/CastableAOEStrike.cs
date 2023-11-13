using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableAOEStrike : MonoBehaviour
{
    public float attackRadius = 10f;
    public float damage = 60f;
    public GameObject projectionPrefab;
    public GameObject particlePrefab;
    public GameObject projection;
    public Camera playerCamera;
    private bool projectionOn = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!projectionOn)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                projection = Instantiate(projectionPrefab, Vector3.zero, Quaternion.identity);
                projectionOn = true;

            }
        }
        else if (projectionOn)
        {

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
            {
                projectionOn = false;
                Destroy(projection);
            }

            UpdateProjection();

            if (Input.GetMouseButtonDown(0))
            {
                Strike(projection.transform.position);
            }
        }

    }

    void UpdateProjection()
    {


        // Get the camera's position and forward direction
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 cameraForward = playerCamera.transform.forward;

        // Create a ray from the camera's position in the forward direction
        Ray ray = new Ray(cameraPosition, cameraForward);
        RaycastHit hit;

        // Check if the ray hits something on the specified layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            Vector3 targetPosition = hit.point;
            // Ensure the object stays on the ground by setting its y-coordinate to the hit point's y-coordinate
            targetPosition.y += 0.2f;

            // Set the object's position to the hit point
            projection.transform.position = targetPosition;
        }

        
    }

    public void Strike(Vector3 centre)
    {
        projectionOn = false;

        // Creates Visual Prefab
        InstantiateStrike(projection.transform.position);

        string targetTag = "Player1";

        if (this.tag == "Player1")
        {
            targetTag = "Player2";
        }
        else if (this.tag == "Player2")
        {
            targetTag = "Player1";
        }


        DetectCharacters(projection.transform.position, targetTag);

        Destroy(projection);
    }

    public void InstantiateStrike(Vector3 centre)
    {
        GameObject strike = Instantiate(particlePrefab, centre, Quaternion.identity);

        StartCoroutine(timerCoroutine(strike));
    }

    public void DetectCharacters(Vector3 centre, string targetTag)
    {
        Collider[] colliders = Physics.OverlapSphere(centre, attackRadius);
        List<GameObject> players = new List<GameObject>();

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                players.Add(collider.gameObject);
            }
        }

        foreach (var player in players)
        {
            AttributeManager attributes = player.GetComponent<AttributeManager>();

            if (attributes != null)
            {
                // Calculate the distance between the center and the player
                float distance = Vector3.Distance(centre, player.transform.position);

                float damageMultiplier = damage / attackRadius;

                // Adjust the damage based on distance (you can use any formula here)
                float adjustedDamage = damage - distance * damageMultiplier;

                // Make sure the adjusted damage is not negative
                adjustedDamage = Mathf.Max(0, adjustedDamage);

                // Apply the adjusted damage to the player
                attributes.TakeDamage(adjustedDamage);
            }
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private IEnumerator timerCoroutine(GameObject strike)
    {

        yield return new WaitForSeconds(2f);
        Destroy(strike);
    }
}
