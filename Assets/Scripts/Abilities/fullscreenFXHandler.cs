using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenFXHandler : MonoBehaviour
{
    [SerializeField] private RawImage fire;
    [SerializeField] private RawImage ice;
    [SerializeField] private RawImage lightning;
    [SerializeField] private RawImage wind;
    [SerializeField] private RawImage lowhealth;


    public void ToggleFireOn()
    {
        fire.enabled = true;
        ice.enabled = false;
        lightning.enabled = false;
        wind.enabled = false;
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

    public void ToggleLowHealth()
    {
        lowhealth.enabled = true;
        fire.enabled = false;
        ice.enabled = false;
        lightning.enabled = false;
        wind.enabled = false;
    }

    public void StopLowHealth()
    {
        lowhealth.enabled = false;
    }

    public void ToggleEffectsOff()
    {
        fire.enabled = false;
        ice.enabled = false;
        lightning.enabled = false;
        wind.enabled = false;
    }
}
