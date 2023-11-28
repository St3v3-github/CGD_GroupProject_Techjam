using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastOnRay : MonoBehaviour
{
    public GameObject spell;
    public Camera playerCam;
    public int range;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Cast();
        }
    }

    public void Cast()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range))
        {
            // Instantiate the object at the hit point
            Instantiate(spell, hit.point, Quaternion.identity);
        }
    }
}

