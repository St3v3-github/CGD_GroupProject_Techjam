using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject hitMarker;
    public GameObject scoreboard;
    public GameObject player;
    public TextMeshProUGUI healthText;


    private void Update()
    {
        float health = player.GetComponent<AttributeManager>().currentHealth;
        healthText.text = health.ToString();
    }
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
