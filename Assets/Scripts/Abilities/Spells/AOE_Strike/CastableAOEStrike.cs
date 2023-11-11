using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableAOEStrike : MonoBehaviour
{
    public float attackRadius = 10f;
    public GameObject projectionPrefab;
    public GameObject particlePrefab;
    public GameObject projection;
    public Camera playerCamera;
    public int state = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centre = GetMouseWorldPosition();

        if (state == 0)
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                projection = Instantiate(projectionPrefab, Vector3.zero, Quaternion.identity);
                state = 1;

            }
        }
        else if (state == 1)
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


                // Move the specified object to the point where the ray hits the ground
                UpdateProjection(projection, hit.point);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                state = 0;
                
                Strike(projection.transform.position);

                DetectCharacters(projection.transform.position);

                Destroy(projection);
            }
        }

    }

    void UpdateProjection(GameObject obj, Vector3 targetPosition)
    {
        // Ensure the object stays on the ground by setting its y-coordinate to the hit point's y-coordinate
        targetPosition.y += 0.2f;

        // Set the object's position to the hit point
        obj.transform.position = targetPosition;
    }

    public void Strike(Vector3 centre)
    {
        GameObject strike = Instantiate(particlePrefab, centre, Quaternion.identity);

        StartCoroutine(timerCoroutine(strike));
    }

    public void DetectCharacters(Vector3 centre)
    {
        Collider[] colliders = Physics.OverlapSphere(centre, attackRadius);
        List<GameObject> players = new List<GameObject>();

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                players.Add(collider.gameObject);
            }
        }

        foreach (var player in players)
        {
//            Debug.Log("Detected player: " + player.name);

            //damage here
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
