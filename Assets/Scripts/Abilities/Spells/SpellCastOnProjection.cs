using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastOnProjection : Spell
{
    public GameObject spellPrefab;
    public GameObject projectionPrefab;
    
    private GameObject projection;
    public Camera playerCamera;
    public bool projectionOn = false;


    // Start is called before the first frame update
    void Start()
    {
        


       // source = gameObject;

    }


    // Update is called once per frame
    void Update()
    {

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

    public override void Cast()
    {
        projectionOn = false;
        Destroy(projection);

        // Creates Visual Prefab
        InstantiateSpell(projection.transform.position);

        
    }

    public void InstantiateSpell(Vector3 centre)
    {
        GameObject spell = Instantiate(spellPrefab, centre + Vector3.up * 0.5f, Quaternion.identity);
        StartCoroutine(timerCoroutine(spell));
//        AudioManager.instance.PlayOneShot(FMODEvents.instance.thunderSound, this.transform.position);
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
