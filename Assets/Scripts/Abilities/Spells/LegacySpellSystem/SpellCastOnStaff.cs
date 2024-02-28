using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellCastOnStaff : MonoBehaviour
{
    public GameObject spell;
    public Transform playerCam;
    public Transform FirePoint;
    public int range;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cast();
        }
    }

    public void Cast()
    {

         // Instantiate the object on staff
         GameObject bh = Instantiate(spell, FirePoint.position, Quaternion.identity);
         bh.transform.rotation = playerCam.transform.rotation;
    }
}
