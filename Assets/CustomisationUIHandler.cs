using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using static CharSetup;

public class CustomisationUIHandler : MonoBehaviour
{
    public string[] classNames;
    public string[] partNames;
    public TextMeshProUGUI[] textObjects;
    public TextMeshProUGUI classText;

    public GameObject[] ClassButtons;
    public GameObject[] HairButtons;
    public GameObject[] HeadButtons;
    public GameObject[] TorsoButtons;
    public GameObject[] LegButtons;
    public GameObject[] ExitButtons;
    public Material notSelectedMat;
    public Material selectedMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSelectionNames(int[] selected, int classSelection)
    {
        for(int i = 0; i<textObjects.Count();i++)
        {
            textObjects[i].text = partNames[i] + (selected[i] + 1).ToString();
        }
        classText.text = classNames[classSelection];
    }


    public void UpdateUISeclections(CharMenuLevels charMenuLevel, int selectedUI)
    {
        foreach (var button in ClassButtons)
            {
            button.GetComponent<Renderer>().material = notSelectedMat;
        }
        foreach (var button in HairButtons)
            {
            button.GetComponent<Renderer>().material = notSelectedMat;
        }
        foreach (var button in HeadButtons)
            {
            button.GetComponent<Renderer>().material = notSelectedMat;
        }
        foreach (var button in TorsoButtons)
        {
            button.GetComponent<Renderer>().material = notSelectedMat;
        }
        foreach (var button in LegButtons)
        {
            button.GetComponent<Renderer>().material = notSelectedMat;
        }
        foreach (var button in ExitButtons)
        {
            button.GetComponent<Renderer>().material = notSelectedMat;
        }







        // SWITCH FOR CURRENT ACTIVE MENU OPTIONS ENUM
        // DISABLE ALL HIGHLIGHTS AND ENABLE THE SELECTEDUI OPTION'S HIGHLIGHT

        switch (charMenuLevel)
        {
            case CharMenuLevels.CHAR_CLASS:
                ClassButtons[selectedUI].GetComponent<Renderer>().material = selectedMat;
                break;
            case CharMenuLevels.HAIR_CUSTOM:
                HairButtons[selectedUI].GetComponent<Renderer>().material = selectedMat;
                break;
            case CharMenuLevels.HEAD_CUSTOM:
                HeadButtons[selectedUI].GetComponent<Renderer>().material = selectedMat;
                break;
            case CharMenuLevels.BODY_CUSTOM:
                TorsoButtons[selectedUI].GetComponent<Renderer>().material = selectedMat;
                break;
            case CharMenuLevels.LEGS_CUSTOM:
                LegButtons[selectedUI].GetComponent<Renderer>().material = selectedMat;
                break;
            case CharMenuLevels.MAIN_CONTROLS:
                ExitButtons[selectedUI].GetComponent<Renderer>().material = selectedMat;
                break;
            case CharMenuLevels.LEVEL_MAX:
                break;
            default:
                break;
        }




    }
}
