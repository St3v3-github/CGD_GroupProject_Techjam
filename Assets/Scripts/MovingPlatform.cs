using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    LayerMask playerLayer;

    public GameObject startPos;
    public GameObject endPos;

    public float speed = 5.0f;
    private bool to = true;
    private float elapsedTime = 0;
    public float delay = 5;

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        float step = speed * Time.deltaTime;

        if (elapsedTime > delay)
        {
            if (to)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos.transform.position, step);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, step);
            }
        }

        if (to)
        {
            if (transform.position == endPos.transform.position)
            {
                to = false;
                elapsedTime = 0;
            }
        }
        else
        {
            if (transform.position == startPos.transform.position)
            {
                to = true;
                elapsedTime = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player On");
            collision.transform.parent.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.parent != null)
        {
            Debug.Log("Player Off");
            collision.transform.parent.SetParent(null);
        }
    }
}
