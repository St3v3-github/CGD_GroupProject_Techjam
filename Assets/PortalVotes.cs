using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalVotes : MonoBehaviour
{
    public int mapVotes = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PORTAL ENTERED");
        if(other.transform.parent.CompareTag("Player"))
        {
            mapVotes++;
            GameObject.FindGameObjectWithTag("VoteHandler").GetComponent<MapSelector>().IncrementVote(gameObject);
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            mapVotes--;
            GameObject.FindGameObjectWithTag("VoteHandler").GetComponent<MapSelector>().DecreaseVote(gameObject);
        }

    }
}
