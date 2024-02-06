using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boop : Spell
{
    // Start is called before the first frame update
    public float DirectHitDamagage;
    public GameObject BoopPrefab;
    void Start()
    {
        StartCoroutine(timerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject boop = Instantiate(BoopPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator timerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}

