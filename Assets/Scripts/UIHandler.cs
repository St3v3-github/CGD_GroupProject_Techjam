using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage CharUI;
    public RawImage TitleUI;
    public RawImage SpellUI;

    [Header("Colors")]
    public Color Char; 
    public Color Title;
    public Color Spell;

    [Header("Char Title")]
    public TextMeshProUGUI TitleText;
    public string TitleName;

    [Header("Sliders")]
    public Slider health;
    public Slider movement;

    [Header("Ultimates")]
    public GameObject Poision;
    public bool PoisionBool; 
    public GameObject Beam;
    public bool BeamBool;
    public GameObject Healing; 
    public bool HealingBool;

    [Header("Player Disconnect")]
    public GameObject PD;
    public bool Disconect; 



    // Start is called before the first frame update
    void Start()
    {
        CharUI.color = Char;
        TitleUI.color = Title;
        SpellUI.color = Spell;
        TitleText.text = TitleName;
        
    }

    // Update is called once per frame
    void Update()
    {
     if (PoisionBool) {Poision.SetActive(true);}
     if (!PoisionBool) {Poision.SetActive(false);}
     if (BeamBool) {Beam.SetActive(true);} 
     if (!BeamBool) {Beam.SetActive(false);} 
     if (HealingBool) {Healing.SetActive(true);}
     if (!HealingBool) {Healing.SetActive(false);}
     if (Disconect) {PD.SetActive(true);}
     if (!Disconect) { PD.SetActive(false);}
    }


}
