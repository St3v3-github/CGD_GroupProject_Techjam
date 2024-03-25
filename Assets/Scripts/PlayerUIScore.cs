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

    public List<Color> colours;
    public List<Material> materials;
    public GameModeHandler gameModeHandler;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (registers.Count < gameModeHandler.players.Count)
        {
            players = gameModeHandler.players;
            registers.Clear();
            foreach (var player in players)
            {
                registers.Add(player.GetComponent<ComponentRegistry>());
            }
        }

        for (int i = 0; i < scores.Count; i++)
        {
            if (registers[i] == null) { return; }
            scores[i].text = registers[i].playerScoreInfo.kill_count.ToString();
            registers[i].GetComponentInChildren<SkinnedMeshRenderer>().material = materials[i];
        }
    }


}
