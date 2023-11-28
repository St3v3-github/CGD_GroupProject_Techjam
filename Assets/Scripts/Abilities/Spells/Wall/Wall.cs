using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Wall : ElementalSpell
{
    public GameObject holographicPrefab;
    public GameObject holographic;
    public bool isPlacingWall;
    private Quaternion holographicInitialRotation;
    private float holographicDespawnTime = 5.0f;
    public Camera playerCamera;
    void Start()
    {
        setStatus();

        if (spell.prefab == null)
        {
            Debug.LogError("wallPrefab is not assigned!");
        }
        if (holographicPrefab == null)
        {
            Debug.LogError("holographicPrefab is not assigned!");
        }
    }

    void Update()
    {

        setStatus();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartPlacingWall();
            Invoke("DespawnHolographic", holographicDespawnTime);
        }

        if (isPlacingWall)
        {
            // Update the position of the holographic preview to follow the mouse cursor
            Vector3 mousePosition = GetMouseWorldPosition();
            holographic.transform.position = new Vector3(mousePosition.x, holographic.transform.position.y, mousePosition.z);

            if (Input.GetMouseButtonDown(0))
            {
                StopCoroutine(UpdateHolographicRotation());
                // Output the initial rotation of the holographic
                Debug.Log("Initial Rotation of Holographic: " + holographicInitialRotation.eulerAngles);


                Debug.Log("Rotation of Spawned Wall: " + spell.prefab.transform.rotation.eulerAngles);
                Quaternion holographicRotation = holographic.transform.rotation;
                WallManager.Instance.SpawnWall(holographic.transform.position, holographicRotation);
                Destroy(holographic);
                isPlacingWall = false;
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

    public void StartPlacingWall()
    {
        // Initialize the holographic preview
        holographic = Instantiate(holographicPrefab, Vector3.zero, Quaternion.identity);
        holographicInitialRotation = holographic.transform.rotation;
        isPlacingWall = true;
        StartCoroutine(UpdateHolographicRotation());

    }

    private IEnumerator UpdateHolographicRotation()
    {
        while (isPlacingWall)
        {
            if (playerCamera != null)
            {
                // Match the rotation of the player's camera around the Y-axis
                Quaternion playerCameraRotation = playerCamera.transform.rotation;
                Vector3 euler = playerCameraRotation.eulerAngles;
                euler.x = 0; // Ensure no rotation around X-axis
                euler.z = 0; // Ensure no rotation around Z-axis
                Quaternion rotationAroundY = Quaternion.Euler(euler);
                holographic.transform.rotation = rotationAroundY;
            }
            yield return null;
        }
    }
    private void DespawnHolographic()
    {
        Destroy(holographic);
        isPlacingWall = false;
    }
}



