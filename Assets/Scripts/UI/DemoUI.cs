using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUI : MonoBehaviour
{
    public GameObject fireUIIcon;
    public GameObject iceUIIcon;
    public GameObject electricUIIcon;

    public GameObject projectileIcon;
    public GameObject wallIcon;
    public GameObject areaIcon;
    public GameObject summonIcon;
    

    

    // Start is called before the first frame update
    void Start()
    {
        fireUIIcon.gameObject.SetActive(false);
        iceUIIcon.gameObject.SetActive(false);
        electricUIIcon.gameObject.SetActive(false);
        projectileIcon.gameObject.SetActive(false);
        wallIcon.gameObject.SetActive(false);
        areaIcon.gameObject.SetActive(false);
        summonIcon.gameObject.SetActive(false);
        

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableFire()
    {
        fireUIIcon.gameObject.SetActive(true);
       
    }

    public void EnableIce()
    {
        iceUIIcon.gameObject.SetActive(true);
       
    }

    public void EnableElectric()
    {
        electricUIIcon.gameObject.SetActive(true);
    }public void EnableProjectile()
    {
        projectileIcon.gameObject.SetActive(true);
    }

    public void EnableWall()
    {
        wallIcon.gameObject.SetActive(true);
    }

    public void EnableArea()
    {
        areaIcon.gameObject.SetActive(true);
    } public void EnableSummon()
    {
        summonIcon.gameObject.SetActive(true);
    }
}
