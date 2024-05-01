using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedOptionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        
    }
}
