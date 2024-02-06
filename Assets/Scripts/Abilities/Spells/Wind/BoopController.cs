using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoopController : MonoBehaviour
{

    private bool isColliding = false;
    public LayerMask m_LayerMask;
    public float knockbackForce = 10.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Awake()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        foreach (Collider c in hitColliders)
        {
            GameObject player = c.gameObject;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb !=null)
            {
                Vector3 direction = player.transform.position - transform.position;
                direction = direction.normalized;
                rb.AddForce(-direction * knockbackForce, ForceMode.Impulse);
            }

        }
    }


}

