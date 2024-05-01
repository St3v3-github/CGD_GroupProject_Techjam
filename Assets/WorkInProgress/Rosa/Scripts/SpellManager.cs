/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private List<ProjectileData> spells = new List<ProjectileData>();
    private ProjectileData equippedSpell;

    private int currentSpellIndex = 0;

    private void OnEnable()
    {
        PickupSpell.OnSpellPickedUp += HandleSpellPickedUp;
    }

    private void OnDisable()
    {
        PickupSpell.OnSpellPickedUp -= HandleSpellPickedUp;
    }

    private void HandleSpellPickedUp(ProjectileData spellObject)
    {
        CollectSpell(spellObject);
    }

    private void CollectSpell(ProjectileData spellObject)
    {
        if(spells.Count < 2)
        {
            //store collected spell
            spells.Add(spellObject);
            Debug.Log("Spell collected: " + spellObject.name);

            //equip if first spell
            if (spells.Count == 1)
            {
                EquipSpell(currentSpellIndex);
            }
        }
        else
        {
            //replace equipped spell with new pickup
            spells[currentSpellIndex] = spellObject;
            Debug.Log("Replaced equipped spell with: " + spellObject.name);
            EquipSpell(currentSpellIndex);
        }
        
    }

    private void EquipSpell(int index)
    {
        if(index >= 0 && index < spells.Count) 
        {
            equippedSpell = spells[index];
            Debug.Log("Equipped spell: " + equippedSpell.name);
        }
    }

    private void Update()
    {
        //check for user input to equip a spell
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchSpell();
        }

        if (Input.GetKeyDown(KeyCode.F) && equippedSpell != null)
        {
            // Pass in the player's position
            GameObject castedProjectile = equippedSpell.CastSpell(transform);

            Debug.Log("Casting Spell: " + equippedSpell.name);

            // Destroy the projectile after casting
            if (castedProjectile != null)
            {
                Destroy(castedProjectile, 0.5f); // Adjust the time as needed
            }
        }
        *//*
                if(Input.GetKeyDown(KeyCode.F) && equippedSpell != null)
                {
                    Debug.Log("About to cast spell: " + equippedSpell.name);

                    // Casts at the player's position but can be changed to staff
                    GameObject castedProjectile = equippedSpell.CastSpell(transform.position);

                    Debug.Log("Casting Spell: " + equippedSpell.name);

                    // Destroy the projectile after casting
                    if (castedProjectile != null)
                    {
                        Destroy(castedProjectile, 2f); // adjust to ability lifetime & remove rigidbody on impact
                    }
                }*//*
    }

    *//*private void CastEquippedSpell(Vector3 castPos)
    {
       //check spell has spell component
       SpellComponent spellComponent = equippedSpell.GetComponent<SpellComponent>();

        if(spellComponent != null )
        {
            //cast
            spellComponent.CastSpell(castPos);
            Debug.Log("Casting: " + equippedSpell.name);
        }
        else
        {
            Debug.LogWarning("equipped spell doesn't have spell component");
        }
    }*//*

    private void SwitchSpell()
    {
        currentSpellIndex = (currentSpellIndex + 1) % 2;
        EquipSpell(currentSpellIndex);
    }
}
*/