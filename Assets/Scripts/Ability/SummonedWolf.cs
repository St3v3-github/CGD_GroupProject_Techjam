using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedWolf : MonoBehaviour
{
    public Animator animator;
    //public ParticleSystem deathParticle;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        animator.SetTrigger("Attack");

        // Code to damage player

        //deathParticle.Play(true);
        StartCoroutine(DeathCoroutine());
    }
    private IEnumerator DeathCoroutine()
    {

        yield return new WaitForSeconds(1.1f);

        Destroy(transform.parent.gameObject);
    }


}
