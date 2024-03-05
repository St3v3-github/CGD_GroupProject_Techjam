using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollPlayer : MonoBehaviour
{
    Collider[] ragDollColliders;
    Rigidbody[] limbRigidbodies;
    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        GetRagdollBits();
        RagdollOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetRagdollBits()
    {
        ragDollColliders = this.GetComponentsInChildren<Collider>();
        limbRigidbodies = this.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollMesh()
    {
        anim.enabled = false;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }

        foreach(Rigidbody rb in limbRigidbodies)
        {
            rb.isKinematic = false;
        }
    }
    void RagdollOff()
    {
        anim.enabled = true;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rb in limbRigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}
