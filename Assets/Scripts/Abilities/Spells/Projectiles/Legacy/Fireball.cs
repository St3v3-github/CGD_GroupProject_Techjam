using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    InputManager inputManager;

    public GameObject projectilePrefab;
    public Transform firePoint;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);
    public float projectileForce = 100f;

    public float abiltyCooldown;
    public bool abilityReady;

    // drag & drop applicable status effect data
    [SerializeField] private StatusEffect_Data _data;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))//(abilityReady) //(inputManager.abilityInput1 && abilityReady)
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position - verticalOffset + firePoint.forward, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            Debug.Log("used");

            Invoke(nameof(ResetAbility1), abiltyCooldown);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered trigger");
        var effectable = other.GetComponent<IEffectable>();

        if(effectable != null)
        {
            effectable.ApplyEffect(_data);
        }

        Destroy(this.gameObject);
    }

    private void ResetAbility1()
    {
        abilityReady = true;
    }
}
