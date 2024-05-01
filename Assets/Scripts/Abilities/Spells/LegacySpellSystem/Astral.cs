using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astral : MonoBehaviour
{
    public GameObject player;
    public GameObject playerMesh;
    public bool active = false;
    public Material astralMaterial;
    private Material originalMaterial;
    private string originalTag;
    private int originalLayer;
    private string astralTag = "Astral";
    private int astralLayer = 9;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !active)
        {

            cast();

        }
        if (Input.GetKeyDown(KeyCode.R) && active)
        {

            uncast();
            StopCoroutine(timerCoroutine());

        }
    }

    public void cast()
    {
        // Saves layer and Tag
        originalTag = player.tag;
        originalLayer = player.layer;
        originalMaterial = playerMesh.GetComponent<Renderer>().material;

        player.tag = astralTag;
        player.layer = astralLayer;
        playerMesh.GetComponent<Renderer>().material = astralMaterial;

        StartCoroutine(timerCoroutine());
        active = true;
        Debug.Log("working");
    }

    public void uncast()
    {
        Debug.Log("uncasting");
        player.tag = originalTag;
        player.layer = originalLayer;
        playerMesh.GetComponent<Renderer>().material = originalMaterial;
        active = false;
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(5f);
        uncast();
    }
}
