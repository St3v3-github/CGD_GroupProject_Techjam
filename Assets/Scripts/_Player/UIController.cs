using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        hitMarker.transform.localScale = Vector3.Slerp(Vector3.one / 10, Vector3.one * damage / 10, 0.2f);
        hitMarker.SetActive(true);
        Invoke("ResetHit", 0.2f);
    }

    public void ResetHit()
    {
        hitMarker.transform.localScale = Vector3.one;
        hitMarker.SetActive(false);
    }
}
