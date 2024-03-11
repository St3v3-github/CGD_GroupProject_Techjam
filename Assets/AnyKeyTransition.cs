using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AnyKeyTransition : MonoBehaviour
{
    public CinemachineDollyCart dollyCart;


    private List<string> gamepadButtonNames = new List<string>
    {
        "Button0", // A (Xbox), Cross (PlayStation)
        "Button1", // B (Xbox), Circle (PlayStation)
        "Button2", // X (Xbox), Square (PlayStation)
        "Button3", // Y (Xbox), Triangle (PlayStation)
        "Button4", // Left Bumper (Xbox), L1 (PlayStation)
        "Button5", // Right Bumper (Xbox), R1 (PlayStation)
        "Button6", // Back (Xbox), Share (PlayStation)
        "Button7", // Start (Xbox), Options (PlayStation)
        "Button8", // Left Stick Button (Xbox), L3 (PlayStation)
        "Button9", // Right Stick Button (Xbox), R3 (PlayStation)
    };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for any button press on the gamepad
        for (int i = 0; i < 20; i++) // Iterate over a range of possible gamepad buttons
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0 + i)) // Use raw input detection
            {
                Debug.Log("Button pressed: JoystickButton" + i); // Output button name to debug log
                dollyCart.enabled = true;
            }
        }
    }

}
