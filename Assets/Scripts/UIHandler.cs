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
        
    }
}
