using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpell : MonoBehaviour
{
  

    [SerializeField] SpellDataTemplate spell;

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<SpellManagerTemplate>().spellSlotArray[3] = Instantiate(spell);
            other.gameObject.GetComponentInChildren<SpellManagerTemplate>().SetTargetPoints();
            Destroy(this.gameObject);
        }

    }
   
}
