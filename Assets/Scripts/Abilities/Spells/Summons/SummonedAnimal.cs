using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedAnimal : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem deathParticle;
    public StatusEffect statusEffect;
    public float damage = 30f;


    void Start()
    {
        StartCoroutine(timerCoroutine());
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        animator.SetTrigger("Attack");

        // Code to damage player
        if (other.tag == "player1")
        {
            AttributeManager attributes = other.gameObject.GetComponent<AttributeManager>();

            if (attributes != null)
            {
                attributes.TakeDamage(damage, statusEffect);
            }


            deathParticle.Play(true);
            StartCoroutine(DeathCoroutine());
        }

        
    }
    private IEnumerator DeathCoroutine()
    {

        yield return new WaitForSeconds(1.1f);

        Destroy(transform.parent.gameObject);
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(10f);
        Destroy(transform.parent.gameObject);
    }


}
