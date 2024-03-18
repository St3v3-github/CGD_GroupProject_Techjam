using System;
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
    public RawImage Slot1;
    public RawImage Slot2;
    public RawImage Slot3;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerText;


    [Header("Colors")]
    public Color Char;
    public Color Title;
    public Color Spell;
    public Color SpellCD;
    public Color SpellReady;

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

    [Header("Hit Chain")]
    public bool HitConfirmed;
    public bool ChainInEffect;
    public bool restart;
    public int hitchain = 0;
    public RawImage[] Retical;
    public GameObject ReticalParent;
    public Animator Anim;
    public ComponentRegistry componentRegistry;




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
        
       
        if (Disconect) { PD.SetActive(true); }
        if (!Disconect) { PD.SetActive(false); }
        if (HitConfirmed) { Hitchain(); HitConfirmed = false; }
        health.value = componentRegistry.attributeManager.currentHealth;
        scoreText.SetText(componentRegistry.playerScoreInfo.kill_count.ToString());;


    }

     public void Hit()
    {
        HitConfirmed = true; 
    }

    void Hitchain()
    {
        if (ChainInEffect) 
        { 
            restart = true;
            hitchain++; 
          
        }
        
        if (!ChainInEffect) 
        {
            hitchain++;
            ChainInEffect = true;
            StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(2);
            if (!restart) { ChainInEffect = false; reset(); }
            if (restart) { StartCoroutine(Timer()); restart = false; }
        }

        if (hitchain == 1)
        {
            for (int i = 0; i < Retical.Length; i++)
            {
                Retical[i].color = Color.red;

            }
        }

        if (hitchain == 2)
        {
            Anim.Play("Testing Something");
        }

        if (hitchain == 3)
        {
            for (int i = 0; i < Retical.Length; i++)
            {
                Retical[i].color = Color.blue;

            }
        }

        if (hitchain == 4)
        {
            Anim.Play("Testing Something");
        }

        

    }

    void reset()
    {
        hitchain = 0;
        for (int i = 0; i < Retical.Length; i++)
        {
            Retical[i].color = Color.gray;
            
        }
    }

    public void ToggleSlot1(bool onCD)
    {
        if (onCD)
        {
            Slot1.color = SpellCD;
        }
        else
        {
            Slot1.color = SpellReady;
            
        }

    }
    public void ToggleSlot2(bool onCD)
    {
        if (onCD)
        {
            Slot2.color = SpellCD;
        }
        else
        {
            Slot2.color = SpellReady;
        }
        
    }
    public void ToggleSlot3(bool onCD)
    {
        if (onCD)
        {
            Slot3.color = SpellCD;
        }
        else
        {
            Slot3.color = SpellReady;
        }
        
    }

    public void PoisonUlt()
    {
        Poision.SetActive(true);
    }

    public void HealingUlt()
    {
        Healing.SetActive(true);
    }

    public void BeamUlt () 
    { 
      Beam.SetActive(true);
    }

    public void UsedUlt()
    {
        Beam.SetActive(false); 
        Poision.SetActive(false);
        Healing.SetActive(false);
    }

}
