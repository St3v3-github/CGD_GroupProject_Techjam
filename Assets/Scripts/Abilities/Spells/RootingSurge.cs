using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RootingSurge : MonoBehaviour
{

    public float range = 8f; // Range
    public float angle = 30f; // Angle of the cone
    public float damage = 10f; // Damage
    public StatusEffect statusEffect = new Fire();
    public GameObject particlePrefab;
    public Transform playerCam;
    private PlayerController playerCon;

    // Start is called before the first frame update
    void Start()
    {
        playerCon = GetComponent<PlayerController>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerCon.isGrounded)
        {

            Surge();


        }

    }

    void Surge()
    {
        Vector3 spawnLocation = transform.position;
        spawnLocation.y = 0;

        // Finds ground beneath you

        GameObject test;
        //test = Physics.Raycast(transform.position, Vector3.down, playerCon.playerHeight * 0.5f + 0.5f, playerCon.groundLayer);


        // remove up component of angle

        Quaternion originalQuaternion = playerCam.rotation; // Replace this with your quaternion
        Vector3 eulerAngles = originalQuaternion.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;
        Quaternion rootRotation = Quaternion.Euler(eulerAngles);

        // Create Particle prefab

        GameObject roots = Instantiate(particlePrefab, spawnLocation, rootRotation);


        Collider[] players = Physics.OverlapSphere(transform.position, range);

        foreach (Collider player in players)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(playerCam.forward, directionToPlayer);

            // Calculate the angle difference between the camera forward direction and to the player
            Vector3 eulerRotation = playerCam.rotation.eulerAngles;
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(eulerRotation.y, Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg));

            if (angleToPlayer < angle / 2f && angleDifference < angle / 2f)
            {
                if (player.tag == "Player")
                {
                    Debug.Log("hit object: " + player.gameObject.name);
                    AttributeManager attributes = player.gameObject.GetComponent<AttributeManager>();

                    if (attributes != null)
                    {
                        attributes.TakeDamage(damage, statusEffect);
                    }
                }

                

            }
        }
    }
}
