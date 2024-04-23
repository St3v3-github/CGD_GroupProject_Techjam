using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenFXHandler : MonoBehaviour
{
    [SerializeField] private Image fire;
    [SerializeField] private Image ice;
    [SerializeField] private Image lightning;
    [SerializeField] private Image wind;


    public void ToggleFireOn()
    {
        fire.enabled = true;
        ice.enabled = false;
        lightning.enabled = false;
        wind.enabled = false;
        Debug.Log(".");
    }

    public void ToggleIceOn()
    {
        fire.enabled = false;
        ice.enabled = true;
        lightning.enabled = false;
        wind.enabled = false;
    }

    public void ToggleLightningOn()
    {
        fire.enabled = false;
        ice.enabled = false;
        lightning.enabled = true;
        wind.enabled = false;
    }

    public void ToggleWindOn()
    {
        fire.enabled = false;
        ice.enabled = false;
        lightning.enabled = false;
        wind.enabled = true;
    }

    public void ToggleEffectsOff()
    {
        fire.enabled = false;
        ice.enabled = false;
        lightning.enabled = false;
        wind.enabled = false; 
    }
}
