using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : Spell
{
    public float DirectHitDamagage;
    public GameObject SmashPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        GameObject smash = Instantiate(SmashPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator timerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
