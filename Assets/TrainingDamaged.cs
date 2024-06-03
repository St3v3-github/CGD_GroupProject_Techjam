using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDamaged : MonoBehaviour
{
    public MeshRenderer dummyMesh;
    public SphereCollider dummyMeshCol;
    public Material mainMat;
    public Material redMat;
    public float respawnMAXTimer;
    public float respawmTimer;
    public float damagedMAXTimer;
    public float damageTimer;
    // Start is called before the first frame update
    void Start()
    {
        damageTimer = damagedMAXTimer;
        respawmTimer = respawnMAXTimer;
        dummyMesh = this.GetComponent<MeshRenderer>();
        dummyMeshCol = this.GetComponent<SphereCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(damageTimer < damagedMAXTimer)
        {
            damageTimer += Time.deltaTime;
            if( damageTimer >= damagedMAXTimer)
            {
                damageTimer = damagedMAXTimer;
                dummyMesh.enabled = false;
                dummyMeshCol.enabled = false;
                respawmTimer = 0;
            }
        }
        if(respawmTimer < respawnMAXTimer)
        {
            respawmTimer += Time.deltaTime;
            if(respawmTimer >= respawnMAXTimer)
            {
                respawmTimer = respawnMAXTimer;
                dummyMesh.enabled = true;
                dummyMeshCol.enabled = true;
                this.GetComponent<Renderer>().material = mainMat;
            }
        }
       



        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT BY: " + other.name);
if (other.gameObject.layer == 8)
        {
            Debug.Log("HIT DUMMY");
            damageTimer = 0;
            this.GetComponent<Renderer>().material = redMat;
        }
    }
}
