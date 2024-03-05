using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableAOEStrike : Spell
{

    public GameObject projectionPrefab;
    
    private GameObject projection;
    public Camera playerCamera;
    public bool projectionOn = false;
    public bool doesDamage;


    // Start is called before the first frame update
    void Start()
    {
    //    SetStatus();
        

     //   SetTargetTag();
       // source = gameObject;

    }


    // Update is called once per frame
  /*  void Update()
    {
      //  testingSwitch();

       
         if (projectionOn)
        {

           
            UpdateProjection();

          
        }
*/
    }


    /*public Vector3 GetMouseWorldPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }*/

   /* private IEnumerator timerCoroutine(GameObject strike)
    {

        yield return new WaitForSeconds(2f);
        Destroy(strike);
    }*/

