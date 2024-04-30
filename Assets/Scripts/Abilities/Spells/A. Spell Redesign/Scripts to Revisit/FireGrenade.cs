using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grenade : Spell
{
    public float DirectHitDamage;
    public GameObject Prefab;
    public float activeTime;
    private bool spawned = false;
    public float damage;
    public bool effectNeedsDamage = false;
    public StatusEffect_Data effect;

    //private float timer = 0;


    void Start()
    {
        StartCoroutine(timerCoroutine());
        //effect = GetComponentInParent<StatusEffect_Data>();
        Debug.Log("Current Effect: " + effect);
        //setTargetTag();
    }


    void OnTriggerEnter(Collider collision)
    {
        if(!spawned && collision.transform.parent != null && !collision.transform.parent.CompareTag("Player"))

        {

            spawned = true;

            GameObject effect = Instantiate(Prefab, transform.position, Quaternion.identity);

            //effect.GetComponent<Spell>().source = source;

            

            if(effectNeedsDamage)

            {

                effect.GetComponent<ExplodeAfterDuration>().damage = damage;

                effect.GetComponent<ExplodeAfterDuration>().source = source;

                

            }

            else

            {

                effect.GetComponent<DeleteOnTimer>().setupDelete(activeTime);

            }



            if(effect.GetComponent<PoisonCloud>() != null)

            {

                effect.GetComponent<PoisonCloud>().source = source;

            }

            if (effect.GetComponent<FireCircle>() != null)

            {

                effect.GetComponent<FireCircle>().source = source;

            }



            Destroy(gameObject);

        }
       /* if (dealDamage(collision.gameObject, DirectHitDamage))
        {
            Debug.Log("hit player: " + collision.name);
            
        }*/

        


    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
