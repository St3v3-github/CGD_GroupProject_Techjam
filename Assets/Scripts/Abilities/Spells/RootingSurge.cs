/*using System.Collections;
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
    GameObject ground;

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
        spawnLocation.y -= (playerCon.playerHeight * 0.5f);

        // Finds ground beneath you
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerCon.playerHeight * 0.5f + 0.5f, playerCon.groundLayer))
        {
            // The ray has hit something
            ground = hit.collider.gameObject;
            Debug.Log("ground found");
        }

        Quaternion camQuaternion = playerCam.rotation;
        Quaternion groundQuaternion = ground.transform.rotation; 
        Vector3 camEuler = camQuaternion.eulerAngles;
        Vector3 groundEuler = groundQuaternion.eulerAngles;
        Vector3 finalEuler;
        // remove up component of angle
        Quaternion rootRotation;
        if (ground != null)
        {
            
            finalEuler.x = 0f;
            finalEuler.y = camEuler.y;
            finalEuler.z = 0f;
            Debug.Log("ground rotation" + finalEuler.x + "x " + finalEuler.y + "y " + finalEuler.z + "z ");
            rootRotation = Quaternion.Euler(finalEuler);
            Debug.Log("used");
        }
        else
        {
            Quaternion originalQuaternion = playerCam.rotation; 
            Vector3 eulerAngles = originalQuaternion.eulerAngles;
            eulerAngles.x = 0f;
            eulerAngles.z = 0f;
            rootRotation = Quaternion.Euler(eulerAngles);
        }


        // Create Particle prefab

        GameObject roots = Instantiate(particlePrefab, spawnLocation, rootRotation);
        
        /// Figure out how to slope down next time

        roots.transform.Rotate(groundEuler.z, 0f, 0f);

        StartCoroutine(timerCoroutine(roots));



        Collider[] players = Physics.OverlapSphere(transform.position, range);

        foreach (Collider player in players)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(roots.transform.forward, directionToPlayer);

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

    private IEnumerator timerCoroutine(GameObject roots)
    {

        yield return new WaitForSeconds(5f);
        Destroy(roots);
    }
}
*/