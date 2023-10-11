using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    //public Ability ability;
    //Won't need these as we are working with potentially several abilities not just one.
    float cooldown_time;
    float active_time;

    private PlayerControlsAsset player_controls;
    [SerializeField] public List<Ability> ability_list; //if abilities are to be picked up in the world and stored in some kind of inventory. FILO
    private int selected_ability = 0; //selected ability would go from, say, 1-4, if multiple abilities can be held at once.

    enum AbilityState
    {
        READY,
        ACTIVE,
        COOLDOWN
    }

    private void Awake()
    {
        player_controls = new PlayerControlsAsset();
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

    AbilityState state = AbilityState.READY;

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case AbilityState.READY:
                if(player_controls.Player.AbilityCast.IsPressed() && ability_list[selected_ability] != null)
                {
                    ability_list[selected_ability].Activate(gameObject);
                    state = AbilityState.ACTIVE;
                    active_time = ability_list[selected_ability].active_time;
                }
                if(player_controls.Player.AbilityCast.IsPressed() && ability_list[selected_ability] == null)
                {
                    print("Ability does not exist!");
                }
                break;

            case AbilityState.ACTIVE:
                if(active_time > 0)
                {
                    active_time -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.COOLDOWN;
                    cooldown_time = ability_list[selected_ability].ability_cooldown;
                }
                break;

            case AbilityState.COOLDOWN:
                if (cooldown_time > 0)
                {
                    cooldown_time -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.READY;
                }
                break;
        }

        //ADD HANDLING SO PLAYER CANT SELECT NON EXISTENT ELEMENT
        if(player_controls.Player.Ability1.IsPressed())
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
    }
}
