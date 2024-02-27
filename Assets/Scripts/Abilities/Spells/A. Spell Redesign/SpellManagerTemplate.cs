using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellManagerTemplate : MonoBehaviour
{
    public SpellDataTemplate[] spellSlotArray = new SpellDataTemplate[4];

    private SpellDataTemplate spellDataTemplate;

    private void Start()
    {
        spellDataTemplate = FindObjectOfType<SpellDataTemplate>();


        for (int i = 0; i < spellSlotArray.Length; i++)
        {
            spellSlotArray[i] = Instantiate(spellSlotArray[i]);
        }

    }

    private void Update()
    {
        SpellStateManagement(spellSlotArray.Length);
    }


    private void HandleProjectileSpells()
    {

    }

    private void HandleThrowableSpells()
    {


    }

    private void HandleOneStageSpells()
    {



    }

    private void HandleTwoStageSpells()
    {


    }

    private void HandleContinuousSpells()
    {

    }


    private void HandleRaycastSpells()
    {

    }

    #region Spell State Machine

    private void SpellStateManagement(int slot)
    {
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;

        switch (spellSlotArray[slot].currentState)
        {
            case SpellDataTemplate.SpellState.READY:
                HandleSpellReady();
                break;

            case SpellDataTemplate.SpellState.ACTIVE:
                StartCoroutine(SpellActiveTimer(slot, spellDataTemplate.activeTime));
                break;

            case SpellDataTemplate.SpellState.COOLDOWN:
                StartCoroutine(SpellCooldownTimer(slot, spellDataTemplate.activeTime));
                break;

        }
    }

    private void HandleSpellReady()
    {
        if (spellDataTemplate.isReadyState)
        {
            Debug.Log("Spell Ready");

            //Put conditional for moving into active state Here

        }
    }

    private IEnumerator SpellActiveTimer(int slot, float activeTime)
    {
        yield return new WaitForSeconds(spellDataTemplate.activeTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
    }

    private IEnumerator SpellCooldownTimer(int slot, float cooldown)
    {
        yield return new WaitForSeconds(spellDataTemplate.cooldownTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;

    }

    #endregion


    public void Cast(int slotNumber)
    {

        switch (spellSlotArray[slotNumber].ID)
        {
            #region Fire
            case SpellDataTemplate.SpellID.FireProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.FireGrenade:
                HandleThrowableSpells();
                break;

            case SpellDataTemplate.SpellID.FlameThrower:
                HandleContinuousSpells();
                break;

            #endregion


            #region Ice
            case SpellDataTemplate.SpellID.IceProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.IceGrenade:
                HandleThrowableSpells();
                break;

            case SpellDataTemplate.SpellID.IceWall:
                HandleTwoStageSpells();
                break;

            #endregion


            #region Wind
            case SpellDataTemplate.SpellID.WindProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.WhirlwindBouncePad:
                HandleTwoStageSpells();
                break;

            case SpellDataTemplate.SpellID.WindRushKnockback:
                HandleOneStageSpells();
                break;

            #endregion


            #region Lightning
            case SpellDataTemplate.SpellID.LightningProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.LightningStrikeAOE:
                HandleTwoStageSpells();
                break;

            case SpellDataTemplate.SpellID.LightningChainRaycast:
                HandleRaycastSpells();
                break;

            #endregion


            #region Ultimates
            case SpellDataTemplate.SpellID.Beam:
                HandleContinuousSpells();
                break;

            case SpellDataTemplate.SpellID.BlackHole:
                HandleOneStageSpells();
                break;

            case SpellDataTemplate.SpellID.PoisonCloud:
                HandleRaycastSpells();
                break;

            case SpellDataTemplate.SpellID.Heal:
                HandleOneStageSpells();
                break;

                #endregion

        }
    }

    public void EquipUltimate(SpellDataTemplate newSlotData)
    {
        spellSlotArray[3] = newSlotData;
    }

    private void DealDamage()
    {

    }


    //Function to equip ultimate

    //Set Active For function for ultimate

    //Set Cooldown function
}


