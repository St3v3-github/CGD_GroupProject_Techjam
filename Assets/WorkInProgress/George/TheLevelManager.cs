using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLevelManager : MonoBehaviour
{
    public bool isOn = false;
    [SerializeField] public GameObject a;
    public int levelSelect;
    // Start is called before the first frame update
    void Start()
    {
        isOn = true;
        levelSelect = a.GetComponent<LevelSelectController>().MapNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
