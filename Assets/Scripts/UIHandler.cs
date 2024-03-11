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

    [Header("Hit Chain")]
    public bool Hit;
    public bool ChainInEffect;
    public bool restart;
    public int hitchain = 0;
    public RawImage[] Retical;
    public GameObject ReticalParent;
    public Animator Anim; 



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
        if (PoisionBool) { Poision.SetActive(true); }
        if (!PoisionBool) { Poision.SetActive(false); }
        if (BeamBool) { Beam.SetActive(true); }
        if (!BeamBool) { Beam.SetActive(false); }
        if (HealingBool) { Healing.SetActive(true); }
        if (!HealingBool) { Healing.SetActive(false); }
        if (Disconect) { PD.SetActive(true); }
        if (!Disconect) { PD.SetActive(false); }
        if (Hit) { Hitchain(); Hit = false; }
        
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
            Retical[0].color = new Color (250,0,0,255);
        }

        if (hitchain == 2)
        {
            Retical[1].color = new Color(250, 0, 0, 255);
        }

        if (hitchain == 3)
        {
            Retical[2].color = new Color(250, 0, 0, 255);
        }

        if (hitchain == 4)
        {
            Retical[3].color = new Color(250, 0, 0, 255);
        }

        if (hitchain == 5)
        {
            ReticalParent.transform.Rotate (0,0,45,Space.Self);
        }

        if (hitchain == 6)
        {
            Anim.Play("Testing Something"); 
        }
        if (hitchain == 7)
        {
            Retical[0].color = Color.blue;
        }

        if (hitchain == 8)
        {
            Retical[1].color = Color.blue;
        }

        if (hitchain == 9)
        {
            Retical[2].color = Color.blue;
        }

        if (hitchain == 10)
        {
            Retical[3].color = Color.blue;
        }

        if (hitchain == 11)
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
        ReticalParent.transform.Rotate(0, 0, -45, Space.Self);

    }

    

   

    


}
