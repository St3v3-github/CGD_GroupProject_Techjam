using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ChainLightning : Spell
{
    [Header("Player References")]
    public Camera playerCam;

    [Header("Data")]
    public float damage;
    public float bounceDamage;
    public int bounceTotal;
    public float range;
    public float bounceRange;
    public LayerMask hittable;
    private RaycastHit rayHit;
    List<GameObject> playersHit = new List<GameObject>();

    public void Cast()
    {
        List<GameObject> playersHit = new List<GameObject>();
        GameObject lastPlayerHit;
        Vector3 direction = playerCam.transform.forward;
        if (Physics.Raycast(playerCam.transform.position, direction, out rayHit, range, hittable))
        {
            // Intial hit
            if(dealDamage(rayHit.collider.gameObject, damage))
            {
                Debug.Log("Hit");
                playersHit.Add(rayHit.collider.gameObject);
                lastPlayerHit = rayHit.collider.gameObject;

                // Attempt to bounce x number of times
                for (int i = 0; i < bounceTotal; i++)
                {
                    GameObject newPlayer = chain(lastPlayerHit);
                    //Bounced hit
                    if (dealDamage(newPlayer, damage))
                    {
                        Debug.Log("Bounced and Hit");
                        lastPlayerHit = newPlayer;
                        playersHit.Add(newPlayer);
                    }
                }
            }
        }
    }

    public GameObject chain(GameObject struck)
    {
        // Find all other layers within Range and add to List
        Collider[] colliders = Physics.OverlapSphere(struck.transform.position, bounceRange);
        List<GameObject> players = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            if (playerCheck(collider.gameObject))
            {
                players.Add(collider.gameObject);
            }
        }

        GameObject closestPlayer = null;
        float shortestDistance = float.MaxValue;

        //Find Closest Player not already Struck
        foreach (GameObject player in players)
        {
            if (playersHit.Contains(player))
            {
                continue; 
            }

            float distance = Vector3.Distance(struck.transform.position, player.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPlayer = player;
            }
        }

        // Return next player to be struck
        return closestPlayer;
    }
}
