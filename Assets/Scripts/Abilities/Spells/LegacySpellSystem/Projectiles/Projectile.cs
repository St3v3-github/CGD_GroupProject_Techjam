using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : Spell
{
    public float damage;
    public GameObject hitImpact;
    public LayerMask impactLayers;
    public StatusEffect_Data effect;

    //private float timer = 0;


    void Start()
    {
        StartCoroutine(timerCoroutine());
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
        if (DealDamage(collision.gameObject, damage,source))
        {
            collision.GetComponentInParent<ComponentRegistry>().playerScoreInfo.lastDamagedBy = source;
            source.GetComponent<ComponentRegistry>().uiHandler.Hit();
            Debug.Log("hit player: " + collision.name);

            //StatusEffectHandler enemyEffects = collision.gameObject.GetComponent<ComponentRegistry>().statusEffectHandler;

            StatusEffectHandler enemyEffects = collision.gameObject.GetComponentInParent<ComponentRegistry>().statusEffectHandler;
            if(enemyEffects == null)
            {
                Debug.Log("enemy effects null");
            }

            if(enemyEffects != null)           
            {
                Debug.Log("Applied effects");
                enemyEffects.ApplyEffect(effect);             
            }

            //GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(collision.transform.parent.tag != source.tag )
        {
            Debug.Log("hit: " + collision.name);
            Debug.Log("Current Effect: " + effect);
            //GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

       
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
