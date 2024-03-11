using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAfterDuration : MonoBehaviour
{
    public GameObject effectToSpawn;
    public float duration;
    public float damage = 0;
    public GameObject source;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAfterTime());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnAfterTime()
    {
        yield return new WaitForSeconds(duration);
      Instantiate(effectToSpawn,transform.position,Quaternion.identity);
        effectToSpawn.GetComponent<AreaDamageEffect>().damage = damage;
        effectToSpawn.GetComponent<AreaDamageEffect>().duration = duration;
        effectToSpawn.GetComponent<AreaDamageEffect>().source = source;
        Destroy(this.gameObject);


    }
}
