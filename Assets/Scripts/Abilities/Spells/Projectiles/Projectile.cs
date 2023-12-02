using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : Spell
{
    public StatusEffect statusEffect;
    public float damage;
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


        dealDamage(collision.gameObject, damage);

        Destroy(gameObject);
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
