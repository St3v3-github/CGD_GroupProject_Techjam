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
        fireUIIcon.gameObject.active = false;
        iceUIIcon.gameObject.active = false;
        electricUIIcon.gameObject.active = false;
        projectileIcon.gameObject.active = false;
        wallIcon.gameObject.active = false;
        areaIcon.gameObject.active = false;
        summonIcon.gameObject.active = false;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableFire()
    {
        fireUIIcon.gameObject.active = true;
    }

    public void EnableIce()
    {
        iceUIIcon.gameObject.active = true;
    }

    public void EnableElectric()
    {
        electricUIIcon.gameObject.active = true;
    }public void EnableProjectile()
    {
        projectileIcon.gameObject.active = true;
    }

    public void EnableWall()
    {
        wallIcon.gameObject.active = true;
    }

    public void EnableArea()
    {
        areaIcon.gameObject.active = true;
    } public void EnableSummon()
    {
        summonIcon.gameObject.active = true;
    }
}
