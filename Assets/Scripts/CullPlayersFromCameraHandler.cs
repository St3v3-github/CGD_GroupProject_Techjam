using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

public class CullPlayersFromCameraHandler : MonoBehaviour
{
    public int current_player_count = 0;

    public LayerMask p1_layermask;

    public LayerMask p2_layermask;

    public LayerMask p3_layermask;

    public LayerMask p4_layermask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetNewLayer()
    {
        switch (current_player_count)
        {
            case 0:
                return 12;
                break;
            case 1:
                return 13;
                break;
            case 2:
                return 14;
                break;
            case 3:
                return 15;
                break;
            
        }

        return 0;
    }
    public LayerMask GetNewLayerMask()
    {
        switch (current_player_count)
        {
            case 0:
                return p1_layermask;
                break;
            case 1:
                return p2_layermask;
                break;
            case 2:
                return p3_layermask;
                break;
            case 3:
                return p4_layermask;
                break;
            
        }

        return p1_layermask;
    }
}
