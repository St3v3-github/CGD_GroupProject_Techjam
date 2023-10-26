using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputAddition : MonoBehaviour
{
    public int element_count = 4;
    public int spell_count = 4;
    public InputActionMap target;
    public List<string> action_bindings;
    // Start is called before the first frame update
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/Actions.html#using-actions
    void Start()
    {
        for(int i = 0;i<element_count;i++)
        {
            var new_action = target.AddAction("Element" + i.ToString());
            new_action.AddBinding(action_bindings[i]);
        }
        for (int i = 0; i < spell_count; i++)
        {
            var new_action = target.AddAction("Spell" + i.ToString());
            new_action.AddBinding(action_bindings[element_count+i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
