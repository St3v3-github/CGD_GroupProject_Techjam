using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAreaTeleports : MonoBehaviour
{
    public GameObject[] teleportLocations;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetTeleportPosition(int playerID)
    {
        return teleportLocations[playerID].transform.position;
    }
}
