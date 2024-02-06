using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreInfo : MonoBehaviour
{
    public int team = 0;
    public int kill_count = 0;
    public int death_count = 0;
    public GameObject lastDamagedBy = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearAllData()
    {
        kill_count = 0;
        death_count = 0;
        lastDamagedBy = null;
    }
}
