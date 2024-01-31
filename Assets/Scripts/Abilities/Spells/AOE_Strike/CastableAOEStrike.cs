using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableAOEStrike : ElementalSpell
{

    public GameObject projectionPrefab;
    
    private GameObject projection;
    public Camera playerCamera;
    public bool projectionOn = false;


    // Start is called before the first frame update
    void Start()
    {
        setStatus();
        

        setTargetTag();
       // source = gameObject;

    }


    // Update is called once per frame
    void Update()
    {
        testingSwitch();

        if (!projectionOn)
        {
           
        }
        else if (projectionOn)
        {

           
            UpdateProjection();

          
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

    public void switchProjectionOn()
    {
        projection = Instantiate(projectionPrefab, Vector3.zero, Quaternion.identity);
        projectionOn = true;
    }

    public void switchProjectionOff()
    {
        projectionOn = false;
        Destroy(projection);
    }

    public void Strike()
    {
        projectionOn = false;
        Destroy(projection);

        // Creates Visual Prefab
        InstantiateStrike(projection.transform.position);

        DetectCharacters(projection.transform.position, targetTag);

        
    }

    public void InstantiateStrike(Vector3 centre)
    {
        GameObject strike = Instantiate(spell.prefab, centre, Quaternion.identity);
        StartCoroutine(timerCoroutine(strike));
//        AudioManager.instance.PlayOneShot(FMODEvents.instance.thunderSound, this.transform.position);
    }

    public void DetectCharacters(Vector3 centre, string targetTag)
    {
        Collider[] colliders = Physics.OverlapSphere(centre, spell.radius);
        List<GameObject> players = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.name);
            if (playerCheck(collider.gameObject))
            {
                players.Add(collider.gameObject);
            }
        }
       

        foreach (var player in players)
        {
            float distance = Vector3.Distance(centre, player.transform.position);

            float damageMultiplier = spell.damage / spell.radius;

            // Adjust the damage based on distance (you can use any formula here)
            float adjustedDamage = spell.damage - distance * damageMultiplier;

            // Make sure the adjusted damage is not negative
            adjustedDamage = Mathf.Max(0, adjustedDamage);
            Debug.Log("damage calculated");
            dealDamage(player, adjustedDamage,true);
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
