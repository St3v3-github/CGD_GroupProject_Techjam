using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDebug : MonoBehaviour
{
    [Tooltip("How many times per second to update all statistics")]
    [SerializeField] private float refresh_rate = 4;

    private int frame_count = 0;
    private float time = 0;
    private float frames_per_second = 0;
    private float top_speed = 0;
    private PlayerController player;

    private bool debug_enabled = true;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {
        if(debug_enabled)
        {
            //Get the current frames per second
            frame_count++;
            time += Time.deltaTime;
            if (time > 1.0 / refresh_rate)
            {
                frames_per_second = Mathf.Round(frame_count / time);
                frame_count = 0;
                time -= 1.0f / refresh_rate;
            }

            // Calculate the player's top velocity.
            if (player.speed > top_speed)
            {
                top_speed = player.speed;
            }
        }
        else
        {
            return;
        }
    }

    private void OnGUI()
    {
        if(debug_enabled)
        {
            GUI.Box(new Rect(0, 0, 200, 80),
            "FPS: " + frames_per_second + "\n" +
            "Current Speed: " + Mathf.Round(player.speed * 100) / 100 + "\n" +
            "Top Speed: " + Mathf.Round(top_speed * 100) / 100 + "\n");
        }
        else
        {
            return;
        }
    }
}
