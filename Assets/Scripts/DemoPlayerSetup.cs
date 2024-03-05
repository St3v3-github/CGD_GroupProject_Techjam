using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoPlayerSetup : MonoBehaviour
{
    public int player_count = 0;

    public GameObject p1_prefab;
    public GameObject p2_prefab;
    public GameObject p3_prefab;
    public GameObject p4_prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextChar()
    {
        player_count++;
        switch (player_count)
        {
            case 0:
                GetComponent<PlayerInputManager>().playerPrefab = p1_prefab;
                break;
            case 1:
                GetComponent<PlayerInputManager>().playerPrefab = p2_prefab;
                break;
            case 2:
                GetComponent<PlayerInputManager>().playerPrefab = p3_prefab;
                break;
            case 3:
                GetComponent<PlayerInputManager>().playerPrefab = p4_prefab;
                break;
            
        }
    }
}
