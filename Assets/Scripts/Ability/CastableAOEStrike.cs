using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableAOEStrike : MonoBehaviour
{
    public float attackRadius = 10f;
    public GameObject particlePrefab;
    public Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centre = GetMouseWorldPosition();

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Strike(centre);

            DetectCharacters(centre);

            
        }
    }

    private void Strike(Vector3 centre)
    {
        GameObject strike = Instantiate(particlePrefab, centre, Quaternion.identity);

        StartCoroutine(timerCoroutine(strike));
    }

    private void DetectCharacters(Vector3 centre)
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
            Debug.Log("Detected player: " + player.name);

            //damage here
        }
    }

    Vector3 GetMouseWorldPosition()
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
