using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boop : Spell
{
    // Start is called before the first frame update
    public GameObject BoopPrefab;
    public Transform playerCam;

    [Header("Stats")]
    [Range(0, 100)]
    public float DirectHitDamage = 0.0f;
    [Range(0, 100)]
    public float knockbackForce = 10.0f;
    public LayerMask m_LayerMask;
    void Start()
    {
    }

    // Update is called once per framed
    void Update()
    {
        //Visualise.DisplayBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);
    }

    public override void Cast()
    {
        GameObject boop = Instantiate(BoopPrefab, transform.position, playerCam.rotation, transform);
        Destroy(boop, 1f);
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        
        foreach (Collider c in hitColliders)
        {
            GameObject player = c.gameObject;
           // dealDamage(player, DirectHitDamage);
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = player.transform.position - transform.position;
                direction = direction.normalized;
                rb.AddForce(-direction * knockbackForce, ForceMode.Impulse);
            }

        }
    }
}

