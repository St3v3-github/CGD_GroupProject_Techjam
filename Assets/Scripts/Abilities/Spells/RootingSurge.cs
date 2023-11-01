using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RootingSurge : MonoBehaviour
{

    public float range = 5f; // Range
    public float angle = 30f; // Angle of the cone
    public float damage = 10f; // Damage
    public StatusEffect statusEffect = new Fire();
    public GameObject particlePrefab;
    public Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Surge();


        }

    }

    void Surge()
    {
        Vector3 spawnLocation = transform.position;
        spawnLocation.y = 0;

        //remove y component of angle

        //Quaternion.EulerRotation rootAngle = playerCam.rotation;
        //rootAngle.eulerAngles.y = 0;
        GameObject roots = Instantiate(particlePrefab, spawnLocation, playerCam.rotation);

        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D player in players)
        {
            Vector2 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector2.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= angle)
            {
                AttributeManager attributes = player.gameObject.GetComponent<AttributeManager>();

                if (attributes != null)
                {
                    attributes.TakeDamage(damage, statusEffect);
                }

            }
        }
    }
}
