using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedAnimal : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem deathParticle;
    public StatusEffect statusEffect;
    public float damage = 30f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private GameObject Target;


    void Start()
    {
        StartCoroutine(timerCoroutine());
        if (this.tag == "Player1Spell")
        {
            Target = FindTarget("Player2");
        }
        else if (this.tag == "Player2Spell")
        {
           Target = FindTarget("Player1");
        }
    }

    void Update()
    {
        if (Target != null)
        {
            MoveTowardsTarget(Target.transform.position);
        }
        else
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);

            if (this.tag == "Player1Spell")
            {
                Target = FindTarget("Player2");
            }
            else if (this.tag == "Player2Spell")
            {
                Target = FindTarget("Player1");
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        animator.SetTrigger("Attack");

        // Code to damage player
        if (other.tag == this.tag)
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

    GameObject FindTarget(string tag)
    { 
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objectsWithTag)
        {
            float distance = Vector3.Distance(obj.transform.position, currentPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        // Add code to only work when within line o site

        Debug.Log(closestObject.name);
        return closestObject;
    }

    void MoveTowardsTarget(Vector3 targetPosition)
    {
        // Calculate the direction from the current position to the target position
        Vector3 direction = targetPosition - transform.position;

        // Calculate the rotation towards the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target using Quaternion.Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards the target position using Vector3.MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
