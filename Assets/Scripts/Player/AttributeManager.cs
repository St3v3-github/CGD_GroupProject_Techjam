using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct killing_data
{
    public GameObject killer;
    public GameObject deaded;
}
public class AttributeManager : MonoBehaviour
{
    //should be private for final (as we use getters and setters) but keep public for dev so we can eyeball inspector
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] public int mp;
    [SerializeField] public float speed;
    [SerializeField] public int score;
    public bool dead = false;
    public bool hit_by_strike = false;
    public bool initialSpawnSetup = true;


    //examples of other values we might eventually have. all values relating to the player would probably be held in this one manager
    //[SerializeField] private int move_speed;
    //[SerializeField] private int defensive_power;
    //[SerializeField] private int offensive_power;/./

    //CURRENT STATUS HERE
    public StatusEffect playerStatus;
    public GameObject lastDamagePlayer;

    public Slider healthbar;
    public GameObject ScoreText;
    public string scorefloat;

    public GameObject damageFlyTextPrefab;
    Color originalColor;

    public DamageFlash damageFlash;
    public CameraShake cameraShake;
    //public FullScreenController fullScreenController;
    public FullScreenFXHandler fullScreenFXHandler;

    public ComponentRegistry componentRegistry;
    void Start()
    {
        //set all values to whatever default value we want
        currentHealth = maxHealth;
        maxHealth = 100;
        mp = 100;

        //need to load from resources as attribute controller seems to be created at runtime, cannot reference in inspector
        damageFlyTextPrefab = (GameObject)Resources.Load("prefabs/DamageText", typeof(GameObject));

        //original_color = GetComponentInParent<Renderer>().material.color;
        //originalColor = transform.parent.gameObject.GetComponentInChildren<Renderer>().material.color;
    }

    void Update()
    {
        //attribute manager would check players current status and call status functions here!!
        //example: player is on fire via fire status, HP reduced by 5 every 1 second?

        //healthbar.value = currentHealth;
        //TODO: Fix this for new GameController
        scorefloat = score.ToString(); 
       // ScoreText.GetComponent<TextMeshProUGUI>().text = scorefloat;

        if(currentHealth <= 10)
        {
            //fullScreenController.DamageLowHealth();
            fullScreenFXHandler.ToggleLowHealth();
        }
        else
        {
            //fullScreenController.DamageLowHealthStop();
            fullScreenFXHandler.StopLowHealth();
        }
                

        

        


    }

    public float GetPlayerHealth()
    {
        return currentHealth;
    }
    public void SetPlayerHealth(float _health)
    {
        currentHealth = _health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetPlayerMP()
    {
        return mp;
    }
    public void SetPlayerMP(int _mp)
    {
        mp = _mp;
    }
    public void Reset()
    {
        currentHealth = maxHealth;
    }

    //delete this later fix spells ya dumbass but go to sleep now
    public float TakeDamage(float damage)
    {

        currentHealth -= damage;
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.hitSound, this.transform.position);
        if (currentHealth <= 0)
        {
            
        }

//        Debug.Log("DAMAGE2");

        //damageFlash.DamageFlashing();
        StartCoroutine(cameraShake.Shake(.15f, .1f));

        //Particles and Shaders called here


        return currentHealth;
    }

    public float TakeDamage(float damage, GameObject attacker)
    {
        currentHealth -= damage;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.hitSound, this.transform.position);
        if (currentHealth <= 0)
        {
            //transform.parent.gameObject.GetComponentInChildren<Renderer>().material.color = originalColor;
            Die(attacker);
        }

      //  Debug.Log("DAMAGE1");

        componentRegistry.animationManager.toggleDamagedTrigger();
        //Particles and Shaders called here


        //DAMAGE FLASH REMOVED FOR DEMO
       // damageFlash.DamageFlashing();
        StartCoroutine(cameraShake.Shake(.15f, .1f));

        if (damageFlyTextPrefab)
        {
            DamageFlyText(damage, attacker);
        }

        if (this.isActiveAndEnabled)
        {
            //StartCoroutine(DamageEffect());
        }

        return currentHealth;
    }

    
    public void Die(GameObject killer)
    {
        killing_data data;
        data.killer = killer;
        data.deaded = gameObject.transform.parent.gameObject;
        //GameObject god = GameObject.Find("GameController");
        //god.SendMessage("PrayToGod", data);
        //Invoke("unDie", 0.1f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deathSound, this.transform.position);
       // fullScreenController.DamageLowHealthStop();
    }

    public void unDie()
    {
        currentHealth = maxHealth;
    }


    public float TakeDamage(float damage, StatusEffect statusEffect)
    {

        currentHealth -= damage;
        ChangeStatus(statusEffect);

        //Particles and Shaders called here
        Debug.Log("Health: " + currentHealth);

        //damageFlash.DamageFlashing();
        StartCoroutine(cameraShake.Shake(.15f, .05f));



        if (damageFlyTextPrefab)
        {
            DamageFlyText(damage);
        }

        if(this.isActiveAndEnabled)
        {
            //StartCoroutine(DamageEffect());
        }

        return currentHealth;
    }

    public float Heal(float heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        

        //Particles and Shaders called here


        return currentHealth;
    }

    public float OverHeal(float heal)
    {
        currentHealth += heal;


        //Particles and Shaders called here


        return currentHealth;
    }

    public float ChangeStatus(StatusEffect newStatus)
    {

        if (playerStatus != newStatus)
        {
            playerStatus.RemoveEffect();

            playerStatus = newStatus;

            playerStatus.ApplyEffect();

        }

        return currentHealth;
    }

    public void SpeedModifier(float speedMod)
    {
        speed *= speedMod;
    
    }

    public void DamageFlyText(float damageDealt)
    {
        var damageText = Instantiate(damageFlyTextPrefab, transform.position, 
            transform.parent.gameObject.GetComponentInChildren<Camera>().transform.rotation, transform);
        damageText.GetComponent<TextMesh>().text = damageDealt.ToString();
    }

    public void DamageFlyText(float damageDealt, GameObject attacker)
    {
        var damageText = Instantiate(damageFlyTextPrefab, transform.position,
            transform.parent.gameObject.GetComponentInChildren<Camera>().transform.rotation, transform);
        damageText.GetComponent<DamageFlyText>().FaceAttacker(attacker.GetComponentInChildren<Camera>());
        //damageText.transform.LookAt(attacker.GetComponentInChildren<Camera>().transform);
        //damageText.transform.rotation = Quaternion.LookRotation(attacker.GetComponentInChildren<Camera>().transform.position - damageText.transform.position);
        //Vector3 dir = attacker.transform.position - damageText.transform.position;
        //damageText.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        damageText.GetComponent<TextMesh>().text = damageDealt.ToString();
    }

    //public IEnumerator DamageEffect()
    //{
     //   transform.parent.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
    //    yield return new WaitForSeconds(0.5f);
    //    transform.parent.gameObject.GetComponentInChildren<Renderer>().material.color = originalColor;
   // }
}
