using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : Spell
{
    public float damage;
    public GameObject hitImpact;
    public LayerMask impactLayers;
    //public StatusEffect_Data effect;

    //private float timer = 0;


    void Start()
    {
        StartCoroutine(timerCoroutine());
        //effect = GetComponentInParent<StatusEffect_Data>();
        //Debug.Log("Current Effect: " + effect);
        //setTargetTag();
    }


    void OnTriggerEnter(Collider collision)
    {
        /*if (dealDamage(collision.transform.gameObject, damage))
        {
            Debug.Log("hit player");
            Destroy(gameObject);

        }
        else*/
        if (DealDamage(collision.gameObject, damage))
        {
            Debug.Log("hit player: " + collision.name);
            //GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag != source.tag)
        {
            Debug.Log(collision.name);
            GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        /*if ((impactLayers | (1 << collision.gameObject.layer)) != 0)
        {
            GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        



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

    public void setLifetime(float lifetime)
    {
        Destroy(gameObject, lifetime);
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
