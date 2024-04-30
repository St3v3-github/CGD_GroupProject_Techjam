using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.UI;


public class PlayerUIScore : MonoBehaviour
{
    public List<GameObject> players;
    public List<ComponentRegistry> registers;
    public List<Text> scores;
    public List<Text> scoresInUse;
    public GameObject SegmentParent2P;
    public GameObject SegmentParent4P;
    public List<Image> segments;



    public List<Color> colours;
    public List<Material> materials;
    public GameModeHandler gameModeHandler;
    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 4; i++)
       // {
        //    UI_Colour[i].GetComponent<Image>().color = colours[i];
       // }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (registers.Count < gameModeHandler.players.Count)
        {
            players = gameModeHandler.players;
            registers.Clear();
            int num = 0;
            foreach (var player in players)
            {
                registers.Add(player.GetComponent<ComponentRegistry>());
                scoresInUse.Add(scores[num]);
                num++;
            }
            segments = SegmentParent2P.GetComponentsInChildren<Image>().ToList();
            int countCycle = 0;
            foreach (Image segment in segments)
            {
                segment.color = colours[countCycle];
                countCycle++;
            }
            segments = SegmentParent4P.GetComponentsInChildren<Image>().ToList();
            countCycle = 0;
            foreach (Image segment in segments)
            {
                segment.color = colours[countCycle];
                countCycle++;
            }
        }

        if (registers.Count > 2)
        {
            SegmentParent4P.SetActive(true);
            SegmentParent2P.SetActive(false);
        }
        if (registers.Count == 2)
        {
            SegmentParent4P.SetActive(false); SegmentParent2P.SetActive(true);
        }

        if (scores.Count > 0)
        {
            for (int i = 0; i < scoresInUse.Count; i++)
            {
                if (registers[i] == null || scoresInUse[i] == null) { return; }
                scoresInUse[i].text = registers[i].playerScoreInfo.kill_count.ToString();
               // registers[i].mainMesh.GetComponent<SkinnedMeshRenderer>().material = materials[i];
            }
        }

        for (int i = 0; i < scores.Count; i++)
        {
            if (i < scoresInUse.Count)
            {
                scores[i].gameObject.SetActive(true);
            }
            else
            {
                scores[i].gameObject.SetActive(false);
            }
        }

    }


}
