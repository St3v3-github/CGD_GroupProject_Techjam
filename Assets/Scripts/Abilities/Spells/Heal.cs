using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public GameObject player;
    public GameObject healEffectPrefab;
    public GameObject healAuraEffectPrefab;
    private GameObject healEffect;
    private GameObject healAuraEffect;

    private AttributeManager attributes;
    private int initialHealAmount = 20;
    private int overTimeHealAmount = 30;
    private float overTimeDuration = 5f;
    private float timeBetweenHeals; // time between each heal increment

    private bool isHealing = false;

    private void Start()
    {
        // Assuming this script is attached to the player GameObject
        attributes = player.GetComponent<AttributeManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && !isHealing)
        {
            StartHealing();
        }
    }

    private void StartHealing()
    {
        if (attributes != null)
        {
            StartCoroutine(HealOverTime());
        }
        else
        {
            Debug.LogError("AttributeManager not found on the player GameObject!");
        }
    }

    private System.Collections.IEnumerator HealOverTime()
    {
        isHealing = true;
        Debug.Log("Starting healing over time...");

        // Initial healing
        attributes.Heal(initialHealAmount);

        Vector3 SpawnOffset = new Vector3(0f, -1f, 0f);
        healEffect = Instantiate(healEffectPrefab, transform.position + SpawnOffset, Quaternion.identity); // Instantiate the "heal" prefab
        healEffect.transform.SetParent(transform);


        Debug.Log("Initial healing: " + initialHealAmount + " health");

        yield return new WaitForSeconds(1f);

        // Destroy the "heal" prefab and Make Aura
        Destroy(healEffect);
        healAuraEffect = Instantiate(healAuraEffectPrefab, transform.position, Quaternion.identity); // Instantiate the "healAura" prefab
        healAuraEffect.transform.SetParent(transform);

        // Calculate the rime between increments and number of increments
        timeBetweenHeals = overTimeDuration / overTimeHealAmount;
        int numberOfIncrements = Mathf.CeilToInt(overTimeDuration / timeBetweenHeals);

        // Distribute the healing over time
        for (int i = 0; i < numberOfIncrements; i++)
        {
            yield return new WaitForSeconds(timeBetweenHeals);
            attributes.Heal(1);
            Debug.Log("Healing increment " + (i + 1) + ": 1 health");
        }

        Debug.Log("Healing over time completed.");
        Destroy(healAuraEffect);
        isHealing = false;
    }
}
