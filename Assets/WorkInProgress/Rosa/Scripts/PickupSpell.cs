using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpell : MonoBehaviour
{
  

    [SerializeField] SpellDataTemplate spell;

    private void OnTriggerEnter(Collider other)
    {
       if(other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Player"))
        {
            this.transform.parent.transform.parent.GetComponent<SpawnItem>().hasItem = false;
            other.gameObject.GetComponentInChildren<SpellManagerTemplate>().spellSlotArray[3] = Instantiate(spell);
            other.gameObject.GetComponentInChildren<SpellManagerTemplate>().SetTargetPoints();
            var uiHandler = other.gameObject.GetComponentInParent<ComponentRegistry>().uiHandler;
            uiHandler.UsedUlt();
            switch (spell.ID)
            {
                case SpellDataTemplate.SpellID.Beam:
                    uiHandler.BeamUlt();
                    break;
                case SpellDataTemplate.SpellID.Heal:
                    uiHandler.HealingUlt();
                    break;
                case SpellDataTemplate.SpellID.PoisonCloud:
                    uiHandler.PoisonUlt();
                    break;
                
            }
            Destroy(this.transform.parent.gameObject);
        }

    }
   
}
