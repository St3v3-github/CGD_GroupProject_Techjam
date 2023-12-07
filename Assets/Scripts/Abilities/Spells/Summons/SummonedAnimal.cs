using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedAnimal : Spell
{
    public Animator animator;
    public ParticleSystem deathParticle;
    public StatusEffect currentStatus;
    public float moveSpeed = 5f;
    public float rotationSpeed = 1f;
    public float visionAngle = 60f;

    private GameObject target;
    public bool hasTarget = false;


    void Start()
    {
        SetTargetTag();


        StartCoroutine(timerCoroutine());

        if (target == FindTarget(targetTag))
        {
            hasTarget = true;
        }
    }

    void Update()
    {
        if (target != null && hasTarget)
        {
            MoveTowardsTarget(target.transform.position);
        }
        else
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (target = FindTarget(targetTag))
            {
                hasTarget = true;
            }

        }
    }

    

    void OnCollisionEnter(Collision collision)
    {
     Debug.Log("THE SUMMON HAS COLLIDED");   
        // Damage
        if (collision.gameObject.tag == targetTag)
        {
        Debug.Log("THE SUMMON HAS COLLIDED WITH ITS TARGET");   
           // animator.SetTrigger("Attack");
           // Debug.Log("Collision detected with: " + collision.gameObject.name);

            DealDamage(collision.gameObject, spell.damage);


            deathParticle.Play(true);
            StartCoroutine(DeathCoroutine());
        }

        // Die if hit wall
        if (collision.gameObject.layer == 8)
        {
            animator.SetTrigger("Attack");
            deathParticle.Play(true);
            StartCoroutine(DeathCoroutine());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("THE SUMMON HAS COLLIDED");   
        // Damage
        if (other.gameObject.tag == targetTag)
        {
            Debug.Log("THE SUMMON HAS COLLIDED WITH ITS TARGET");
            Debug.Log(other.name);
            // animator.SetTrigger("Attack");
            // Debug.Log("Collision detected with: " + collision.gameObject.name);

            DealDamage(other.gameObject, spell.damage);


            deathParticle.Play(true);
            StartCoroutine(DeathCoroutine());
        }

        // Die if hit wall
        if (other.gameObject.layer == 8)
        {
            animator.SetTrigger("Attack");
            deathParticle.Play(true);
            StartCoroutine(DeathCoroutine());
        }
    }


    GameObject FindTarget(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        foreach (GameObject obj in objectsWithTag)
        {
            Vector3 directionToObject = obj.transform.position - currentPosition;
            float angle = Vector3.Angle(forwardDirection, directionToObject);

            // Check if the object is within the field of view angle
            if (angle <= visionAngle * 0.5f)
            {
                float distance = directionToObject.magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }
        
        return closestObject;
    }

    void MoveTowardsTarget(Vector3 targetPosition)
    {

        // Calculate the direction from the current position to the target
        Vector3 directionToTarget = targetPosition - transform.position;

        // Calculate the rotation required to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate towards the target using Quaternion.Lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards the target position
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }


    private IEnumerator DeathCoroutine()
    {

        yield return new WaitForSeconds(1.1f);

        Destroy(transform.gameObject);
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(10f);
        Destroy(transform.gameObject);
    }

    public void SetSpellData(SpellData spellData)
    {
        spell = spellData;
    }
}


