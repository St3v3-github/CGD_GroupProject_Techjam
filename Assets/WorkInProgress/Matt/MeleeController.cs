using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    InputManager inputManager;

    [Header("Melee")]
    public float meleeDistance = 3f;
    public int meleeDamage = 1;
    public float meleeSpeed = 1f;
    public float meleeDelay = 1f;
    public LayerMask playerLayer;


    bool inMelee = false;
    bool readyToMelee = true;

    [Header("Put cam here")]
    public Camera cam;

    
    

    // Start is called before the first frame update
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
/*        if (inputManager.meleeInput == true)
        {
            
            HandleMelee();
        }*/
    }

    private void HandleMelee()
    {

        if (!readyToMelee || inMelee) return;

        readyToMelee = false;
        inMelee = true;

        Invoke(nameof(ResetMelee), meleeSpeed);
        Invoke(nameof(MeleeRaycast), meleeDelay);

        Debug.Log("Melee!");
    }

    private void MeleeRaycast()
    {
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, meleeDistance, playerLayer))
        {
            Debug.Log("HIT ENEMY :O -1 HEALTH");
            if(hit.transform.TryGetComponent<TestDummy>(out TestDummy T))
            {
                T.TakeDamage(meleeDamage);
            }

        }
    }
    
    private void ResetMelee()
    {
        inMelee = false;
        readyToMelee = true;
    }
}
