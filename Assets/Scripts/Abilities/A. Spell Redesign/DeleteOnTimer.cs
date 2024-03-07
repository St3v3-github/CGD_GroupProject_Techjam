using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DeleteOnTimer : MonoBehaviour
{
    private float activeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupDelete(float newActiveTime)
    {
        activeTime = newActiveTime;
        StartCoroutine(DeleteAfterTime());
    }

    private IEnumerator DeleteAfterTime()
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(this.gameObject);
    }
}
