using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CullPlayerFromCamera : MonoBehaviour
{
    public GameObject PlayerCharacter;
    public GameObject PlayerStaff;
    // Start is called before the first frame update
    void Start()
    {
        //var camerahandler = GameObject.FindWithTag("CAMERACULL").GetComponent<CullPlayersFromCameraHandler>();
        //PlayerCharacter.layer = camerahandler.GetNewLayer();
        //this.GetComponent<Camera>().cullingMask = camerahandler.GetNewLayerMask();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
