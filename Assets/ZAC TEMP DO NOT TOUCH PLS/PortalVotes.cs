using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalVotes : MonoBehaviour
{
    public int mapVotes = 0;
    public List<GameObject> gems;
    public Material gemsOn;
    public Material gemsOff;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var gem in gems)
        {
            gem.GetComponent<Renderer>().material = gemsOff;
        }    
        for (int i = 0; i < mapVotes; i++)
        {
            gems[i].GetComponent<Renderer>().material = gemsOn;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PORTAL ENTERED");
        if (other.transform.parent.CompareTag("Player"))
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
