using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject hitMarker;
    public GameObject scoreboard;

    public void OpenScoreboard()
    {
        scoreboard.SetActive(true);
    }
    public void CloseScoreboard()
    {
        scoreboard.SetActive(true);
    }

    public void Hit(float damage)
    {
        hitMarker.transform.localScale = Vector3.one * damage;
        hitMarker.SetActive(true);
        Invoke("ResetHit", 1f);
    }

    public void ResetHit()
    {
        hitMarker.transform.localScale = Vector3.one;
        hitMarker.SetActive(true);
    }
}
