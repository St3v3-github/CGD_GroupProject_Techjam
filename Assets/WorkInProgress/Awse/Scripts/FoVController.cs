using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoVController : MonoBehaviour
{
    //[SerializeField] public GameObject player;
    [SerializeField] public Camera _camera;
    [SerializeField] public float player_speed;
    [SerializeField] public float last_player_speed;
    [SerializeField] public float current_fov;
    [SerializeField] public float desired_fov;
    [SerializeField] const float zoom_step = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        current_fov = 60f;
        desired_fov = current_fov;
    }

    void CheckPlayerSpeed()
    {
        if(player_speed < last_player_speed)
        {
            last_player_speed = player_speed;
            desired_fov = 60f;
        }
        else if (player_speed > last_player_speed)
        {
            last_player_speed = player_speed;
            desired_fov = 100f;
        }
    }

    void ProcessFoV()
    {
        current_fov = Mathf.MoveTowards(current_fov, desired_fov, zoom_step * Time.deltaTime);
    }

    void SetFoV()
    {
         _camera.fieldOfView = current_fov;
    }

    // Update is called once per frame
    void Update()
    {
        player_speed = (GameObject.Find("Player").GetComponent<PlayerController>().speed);
        CheckPlayerSpeed();
        ProcessFoV();
        SetFoV();
    }
}
