using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadRumbleController : MonoBehaviour
{
    public Gamepad[] gamepads;

    private void Awake()
    {
        // Get the first connected gamepad
        gamepads = Gamepad.all.ToArray();
    }

    public void StartRumble(int playerIndex, float duration, float frequency, float amplitude)
    {
        if (playerIndex < 0 || playerIndex >= gamepads.Length)
        {
            Debug.LogWarning("Invalid player index or no gamepad found for player " + playerIndex);
            return;
        }

        for (int i = 0; i < gamepads.Length; i++)
        {
            // Check if the current gamepad matches the specified player index
            if (i == playerIndex)
            {
                // Start the rumble effect for the specified player
                gamepads[i].SetMotorSpeeds(frequency, amplitude);

                // Stop the rumble after the specified duration
                Invoke("StopRumble", duration);
            }

            else
            {
                // Stop the rumble for other gamepads
                gamepads[i].SetMotorSpeeds(0f, 0f);
            }
        }

        // Stop the rumble after the specified duration
        Invoke("StopRumble", duration);
    }

    private void StopRumble()
    {
        // Stop the rumble effect for all connected gamepads
        foreach (var gamepad in gamepads)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}

//Use the below lines of code in whichever script you'd like to start the rumble
//Customizable data invloves (playerIndex, Duration, Frequency, Amplitude)

//rumbleController.StartRumble(0, 1.0f, 1.0f, 0.5f); // Player 1
//rumbleController.StartRumble(1, 1.0f, 1.0f, 0.5f); // Player 2

