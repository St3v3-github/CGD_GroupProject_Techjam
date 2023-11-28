using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellComponent : MonoBehaviour
{
    public virtual void CastSpell(Vector3 castPos)
    {
        //spell logic
        Debug.Log("Casting Spell");
    }
}
