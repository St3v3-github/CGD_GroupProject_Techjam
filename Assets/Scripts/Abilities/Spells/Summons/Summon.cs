using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Summon : MonoBehaviour
{
    public GameObject summonPrefab;
    [SerializeField] private EventReference SpellSummonedSound;
    public Transform summonPoint;
    private Vector3 verticalOffset = new Vector3(0f, 2f, 0f);
    private Vector3 horizontalOffset = new Vector3(1f, 0f, 1f);
    private Quaternion rotation = new Quaternion(0f, 0f, 0f, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Quaternion summonRoation = summonPoint.rotation;
        summonRoation.x = 0f;
        summonRoation.z = 0f;
        GameObject summon = Instantiate(summonPrefab, summonPoint.position - verticalOffset + Vector3.Scale(horizontalOffset, summonPoint.forward), summonRoation);
        summon.tag = this.tag + "Spell";
        AudioManager.instance.PlayOneShot(SpellSummonedSound, this.transform.position);
    }
}
