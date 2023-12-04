using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : ElementalSpell
{
    public Transform summonPoint;
    private Vector3 verticalOffset = new Vector3(0f, 2f, 0f);
    private Vector3 horizontalOffset = new Vector3(1f, 0f, 1f);
    private Quaternion rotation = new Quaternion(0f, 0f, 0f, 1);

    // Start is called before the first frame update
    void Awake()
    {
        //setStatus();

        setTargetTag();
    }

    // Update is called once per frame
    void Update()
    {
        //setStatus();
        if (Input.GetKeyDown(KeyCode.T))
        {
            Cast();
        }
        testingSwitch();
    }

    public override void Cast()
    {
        Quaternion summonRoation = summonPoint.rotation;
        summonRoation.x = 0f;
        summonRoation.z = 0f;
        GameObject summon = Instantiate(spell.prefab, summonPoint.position /*- verticalOffset + Vector3.Scale(horizontalOffset, summonPoint.forward)*/, summonRoation);
        summon.GetComponent<SummonedAnimal>().source = gameObject;
        summon.tag = this.tag + "Spell";
        AudioManager.instance.PlayOneShot(FMODEvents.instance.stagSound, this.transform.position);
    }
}
