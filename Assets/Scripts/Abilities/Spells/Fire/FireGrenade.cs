using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grenade : Spell
{
    public float DirectHitDamage;
    public GameObject Prefab;
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
       /* if (dealDamage(collision.gameObject, DirectHitDamage))
        {
            Debug.Log("hit player: " + collision.name);
            
        }*/

        GameObject effect = Instantiate(Prefab, transform.position, Quaternion.identity);
        effect.GetComponent<Spell>().source = source;
        Destroy(gameObject);


    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
