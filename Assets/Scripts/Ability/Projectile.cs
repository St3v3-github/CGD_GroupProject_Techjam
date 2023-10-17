using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    //private float timer = 0;

    void Start()
    {
        StartCoroutine(timerCoroutine());
    }

    /*void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            Destroy(gameObject);
        }
    }*/
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 3)
        {
            return;
        }


        //Damage here

        Destroy(gameObject);
    }

    private IEnumerator timerCoroutine()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
