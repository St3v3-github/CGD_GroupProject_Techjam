using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpell : MonoBehaviour
{
    public static event System.Action<TestProjectile> OnSpellPickedUp; // Move this line outside the method

    [SerializeField] TestProjectile spell;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("collided");

            // Trigger event passing spell gameobject
            OnSpellPickedUp?.Invoke(spell);

            Destroy(gameObject);
        }
    }
}
