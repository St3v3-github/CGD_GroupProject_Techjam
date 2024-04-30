using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AreaDamageEffect : MonoBehaviour
{
    public float damage;
    public float duration;
    public float radius;
    private List<GameObject> hitObjects = new List<GameObject>();
    public GameObject source;
    // Start is called before the first frame update
    void Start()
    {
        DetectCharacters(transform.position);
       StartCoroutine( DestroyAfterTime());
        
    }

    /*private void OnParticleCollision(GameObject other)
    {
        bool validTarget = true;
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("LIGHTNING GRENADE HAS HIT: " + other.gameObject.name);
            foreach(GameObject obj in hitObjects)
            {
                if(obj == other.gameObject)
                {
                    validTarget = false;
                }
            }

            if(validTarget)
            {
                // DAMAGE LOGIC GOES HERE
                other.GetComponent<ComponentRegistry>().attributeManager.TakeDamage(damage);

            }


           hitObjects.Add(other);
        }
    }*/

    public void DetectCharacters(Vector3 centre)
    {
        Debug.Log("Detect 1");
        Collider[] colliders = Physics.OverlapSphere(centre, radius);
        List<GameObject> players = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            if (PlayerCheck(collider.gameObject))
            {
                bool validTarget = true;
                foreach (GameObject player in players)
                {
                    if(player.gameObject == collider.gameObject)
                    {
                        validTarget = false;
                    }
                }
                 if(validTarget)
                {
                    players.Add(collider.gameObject);
                }
               
            }
        }
        Debug.Log("Detect 2");

        foreach (var player in players)
        {
            player.GetComponentInParent<ComponentRegistry>().attributeManager.TakeDamage(damage);
            player.GetComponentInParent<ComponentRegistry>().playerScoreInfo.lastDamagedBy = source;
            source.GetComponent<ComponentRegistry>().uiController.hitMarker.SetActive(true);
        }
        Debug.Log("Detect 3");
    }

    private IEnumerator DestroyAfterTime()
    {
        Debug.Log("Destroying after time");
        yield return new WaitForSeconds(duration);
         Destroy(this.gameObject);



    }

    public bool PlayerCheck(GameObject hitbox)
    {
        GameObject player = hitbox;

        if (player.layer == LayerMask.NameToLayer("layer_Player"))
        {
            return true;
        }

        return false;
    }

}
