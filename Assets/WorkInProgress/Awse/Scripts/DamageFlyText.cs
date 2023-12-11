using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlyText : MonoBehaviour
{
    public float despawnTime = 2f;
    public Vector3 textOffsetHeight = new Vector3(0, 0.9f, 0);
    public Vector3 randomTextOffset = new Vector3(0.7f, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, despawnTime);
        transform.localPosition += textOffsetHeight;
        transform.localPosition += new Vector3(Random.Range(-randomTextOffset.x, randomTextOffset.x),
            Random.Range(-randomTextOffset.y, randomTextOffset.y), Random.Range(-randomTextOffset.z, randomTextOffset.z));
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
