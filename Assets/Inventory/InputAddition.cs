using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputAddition : MonoBehaviour
{
    public int element_count = 4;
    public int spell_count = 4;
    public InputActionMap target_map;
    public List<string> action_bindings;
    public InputManager target_manager;

    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/Actions.html#using-actions
    void Start()
    {
        for(int i = 0;i < element_count;i++)
        {
            //Create new action
            var new_action = target_map.AddAction("Element" + i.ToString());
            //Add binding to action
            new_action.AddBinding(action_bindings[i]);
            //Activate action and resolve bindings
            new_action.Enable();
            //Add function callback
            new_action.performed += target_manager.OnElement;
            //Profit
        }
        for (int i = 0; i < spell_count; i++)
        {
            var new_action = target_map.AddAction("Spell" + i.ToString());
            new_action.AddBinding(action_bindings[element_count+i]);
        }
    }
}
