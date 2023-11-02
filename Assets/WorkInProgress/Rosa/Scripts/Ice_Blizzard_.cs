using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ice_Blizzard_ : MonoBehaviour
{
    public GameObject blizzardPrefab;
    public GameObject holoPrefab;
    public GameObject holographic;
    public bool isCastingBlizzard;
    public Quaternion initialRotation;
    private float holographicDespawnTime = 3.0f;
    public Camera playerCamera;

    private void Start()
    {
        if(blizzardPrefab == null)
        {
            Debug.LogError("blizzard prefab not assigned");
        }
        if(holoPrefab == null)
        {
            Debug.LogError("holo prefab not assigned");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H) && !isCastingBlizzard)
        {
            StartCastingBlizzard();
        }
        if(isCastingBlizzard)
        {
            //update position of holographic preview
            Vector3 mousePosition = GetMouseWorldPosition();
            holographic.transform.position = new Vector3(mousePosition.x, holographic.transform.position.y, mousePosition.z);
            if(Input.GetKeyDown(KeyCode.M))
            {
                CastBlizzard();
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public void CastBlizzard()
    {
        StopCoroutine(UpdateHolographicRotation());
        Quaternion holographicRotation = holographic.transform.rotation; 

        BlizzardManager.Instance.SpawnBlizzard(holographic.transform.position, holographicRotation);
        Destroy(holographic);
        isCastingBlizzard = false;
    }

    public void StartCastingBlizzard()
    {
        //Initialise holo preview
        holographic = Instantiate(holoPrefab, Vector3.zero, Quaternion.identity);
        initialRotation = holographic.transform.rotation;
        isCastingBlizzard = true;
        StartCoroutine(UpdateHolographicRotation());
        Invoke("DespawnHolographic", holographicDespawnTime);
        
    }

    private IEnumerator UpdateHolographicRotation()
    {
        while(isCastingBlizzard)
        {
            
            if (playerCamera != null)
            {
                //match rotation of player's camera around y-axis
                Quaternion playerCameraRotation = playerCamera.transform.rotation;
                Vector3 euler = playerCameraRotation.eulerAngles;
                euler.x = 0;
                euler.z = 0;//no rotation in x or z axis
                Quaternion rotationAroundY = Quaternion.Euler(euler);
                holographic.transform.rotation = rotationAroundY;
            }
            yield return null;
        }
    }

    private void DespawnHolographic()
    {
        Destroy(holographic);
        isCastingBlizzard = false;
    }
}
