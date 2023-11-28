using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : Spell
{
    public StatusEffect statusEffect;
    //private float timer = 0;


    void Start()
    {
        StartCoroutine(timerCoroutine());
        setTargetTag();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag) || collision.gameObject.layer == 3)
        {
            return;
        }


        AttributeManager attributes = collision.gameObject.GetComponent<AttributeManager>();

        if (attributes != null)
        {
            attributes.TakeDamage(spell.damage, statusEffect);
            attributes.ChangeStatus(statusEffect);
        }

        Destroy(gameObject);
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
