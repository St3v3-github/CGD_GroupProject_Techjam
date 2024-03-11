using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public GameObject source;
    private float damage; // Default 0.2f
    public float activeTime;
    public GameObject deleteTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.parent.CompareTag("Player") && other != source)
        {
            other.GetComponentInParent<ComponentRegistry>().attributeManager.TakeDamage(damage);
            other.GetComponentInParent<ComponentRegistry>().playerScoreInfo.lastDamagedBy = source;

            source.GetComponent<ComponentRegistry>().uiHandler.Hit();
        }
    }

    public void setValues(GameObject newSource, float newActiveTime, float newDamage)
    {
        source = newSource;
        activeTime = newActiveTime;
        damage = newDamage;
        StartCoroutine(DeleteOnTimer());
    }

    private IEnumerator DeleteOnTimer()
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(deleteTarget);
    }

}