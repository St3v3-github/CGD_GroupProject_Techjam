using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

public class PlayerUIScore : MonoBehaviour
{
    public List<GameObject> players;
    public List<ComponentRegistry> registers;
    public List<Text> scores;
    // Start is called before the first frame update
    void Start()
    {
        var ats = FindObjectsOfType<ComponentRegistry>();
        registers = ats.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            if (registers[i] == null) { return; }
            scores[i].text = registers[i].playerScoreInfo.kill_count.ToString();
        }
    }
}
