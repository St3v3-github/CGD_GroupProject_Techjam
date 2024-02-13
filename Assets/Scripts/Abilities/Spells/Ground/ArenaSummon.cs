using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSummon : Spell
{
    public GameObject ArenaPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        GameObject Arena = Instantiate(ArenaPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private IEnumerator timerCoroutine()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
