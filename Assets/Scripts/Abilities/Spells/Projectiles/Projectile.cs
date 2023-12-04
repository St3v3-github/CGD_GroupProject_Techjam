using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : Spell
{
    public float damage;
    //public StatusEffect_Data effect;

    //private float timer = 0;


    void Start()
    {
        StartCoroutine(timerCoroutine());
        //effect = GetComponentInParent<StatusEffect_Data>();
        //Debug.Log("Current Effect: " + effect);
        //setTargetTag();
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.gameObject.layer);
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("layer_Player"))
        {
            Debug.Log("hit player");
            dealDamage(collision.transform.gameObject, damage);
            Destroy(gameObject);

        }
        /*else if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log("hit map");
            Destroy(gameObject);
        }*/
        Debug.Log("hit nothing");


        //AttributeManager attributes = collision.gameObject.GetComponent<AttributeManager>();
        //enemystatuseffects effects = collision.gameObject.GetComponent<enemystatuseffects>();

        /*if (effects != null)
        {
            attributes.TakeDamage(spell.damage, statusEffect);
            attributes.ChangeStatus(statusEffect);

            effects.ApplyEffect(effect);
            Debug.Log("Effect applied");
        }
        else
        {
            Debug.LogError("enemy status effects script not found");
        }*/
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
