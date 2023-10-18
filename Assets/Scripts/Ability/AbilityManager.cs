using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private PlayerControlsAsset player_controls;
    [SerializeField] public List<BaseAbility> ability_list; //if abilities are to be picked up in the world and stored in some kind of inventory. FILO
    private int selected_ability = 0; //selected ability would go from, say, 1-4, if multiple abilities can be held at once.
    private AttributeManager attribute_manager;

    private void Awake()
    {
        player_controls = new PlayerControlsAsset();
        attribute_manager = GetComponent<AttributeManager>();
        //ability_list.Add(ability); //just to test
    }

    private void OnEnable()
    {
        player_controls.Enable();
    }

    private void OnDisable()
    {
        player_controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        AbilityControls();
        AbilityHandling();
        
    }

    void AbilityHandling()
    {
        if (ability_list[selected_ability] == null)
        {
            selected_ability = 1;
        }
        switch (ability_list[selected_ability].state)
        {
            case BaseAbility.AbilityState.READY:
                if (player_controls.Player.AbilityCast.IsPressed() && ability_list[selected_ability] != null)
                {
                    if (ability_list[selected_ability].GetAbilityCost() <= attribute_manager.GetPlayerMP())
                    {
                        ability_list[selected_ability].Activate(gameObject);
                        ability_list[selected_ability].SetAbilityState(BaseAbility.AbilityState.ACTIVE);
                        attribute_manager.SetPlayerMP(attribute_manager.GetPlayerMP() - ability_list[selected_ability].GetAbilityCost());
                        print("Ability Used");
                    }
                    else
                    {
                        print("Not enough MP for ability! MP: " + attribute_manager.GetPlayerMP());
                    }
                }
                if (player_controls.Player.AbilityCast.IsPressed() && ability_list[selected_ability] == null)
                {
                    print("Ability does not exist!");
                }
                break;

            case BaseAbility.AbilityState.ACTIVE:
                if (ability_list[selected_ability].GetAbilityActiveTime() > 0)
                {
                    ability_list[selected_ability].SetAbilityActiveTime(ability_list[selected_ability].GetAbilityActiveTime() - Time.deltaTime);
                    Debug.Log("Ability Active");
                }
                else
                {
                    ability_list[selected_ability].BeginCooldown(gameObject);
                    ability_list[selected_ability].SetAbilityState(BaseAbility.AbilityState.COOLDOWN);
                }
                break;

            case BaseAbility.AbilityState.COOLDOWN:
                if (ability_list[selected_ability].GetAbilityCooldown() > 0)
                {
                    ability_list[selected_ability].SetAbilityCooldown(ability_list[selected_ability].GetAbilityCooldown() - Time.deltaTime);
                    Debug.Log("Ability CD");
                }
                else
                {
                    ability_list[selected_ability].ResetCooldown();
                    ability_list[selected_ability].SetAbilityState(BaseAbility.AbilityState.READY);
                }
                break;
        }
    }

    void AbilityControls()
    {
       //ADD HANDLING SO PLAYER CANT SELECT NON EXISTENT ELEMENT
        if (player_controls.Player.Ability1.IsPressed())
        {
            selected_ability = 0;
            print("SELECTED ABILITY 1");
        }
        if (player_controls.Player.Ability2.IsPressed())
        {
            selected_ability = 1;
            print("SELECTED ABILITY 2");
        }
        if (player_controls.Player.Ability3.IsPressed())
        {
            selected_ability = 2;
            print("SELECTED ABILITY 3");
        }
        if (player_controls.Player.Ability4.IsPressed())
        {
            selected_ability = 3;
            print("SELECTED ABILITY 4");
        }

        if(player_controls.Player.AbilityIncrement.IsPressed())
        {
            selected_ability = selected_ability + 1;
        }
        if (player_controls.Player.AbilityDecrement.IsPressed())
        {
            selected_ability = selected_ability - 1;
        }

        if(selected_ability < 0)
        {
            selected_ability = 0;
        }
        if(selected_ability > 3)
        {
            selected_ability = 3;
        }
    }

    public List<BaseAbility> GetAbilityList()
    {
        return ability_list;
    }
}
