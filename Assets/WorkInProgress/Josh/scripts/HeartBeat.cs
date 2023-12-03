using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class HeartBeat : MonoBehaviour
{
    public Animator animator; 
    public Slider Health;
    public GameObject Text;
    public string HealhString;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         HealhString = Health.value.ToString();
         Text.GetComponent<TextMeshProUGUI>().text = HealhString;
       
        if (Health.value > 75) 
        { 
            animator.speed = 0; 
        }
        if (Health.value < 75) 
        {
            if (Health.value > 50)
            {
                animator.speed = 1;
            }
        }
        if (Health.value < 50)
        {
            if (Health.value > 20)
            {
                animator.speed = 2;

            }
        }

        if (Health.value < 20)
        {
            if (Health.value > 0)
            {
                animator.speed = 3;

            }
        }

    }
}
