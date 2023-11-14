using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    protected float duration = 5f;
    protected GameObject affectedObject;
    public bool active = false;


    public StatusEffect()
    {
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();

    public abstract string GetStatusType();
}

public class Fire : StatusEffect
{
    private float damagePerSecond = 4;

    public Fire() : base()
    {
    }

    public override void ApplyEffect()
    {
        active = true;
        StartCoroutine(FireCoroutine());
    }

    public void ApplyEffect(float duration)
    {
        active = true; 
        this.duration = duration;
        StartCoroutine(FireCoroutine());
    }

    public override void RemoveEffect()
    {
        active = false;
        CancelInvoke("DealDamage");
    }

    public override string GetStatusType()
    {
        return "fire";
    }

    private IEnumerator FireCoroutine()
    {
        InvokeRepeating("DealDamage", 1f, 1f);

        yield return new WaitForSeconds(duration);

        RemoveEffect();
    }

    private void DealDamage()
    {
        AttributeManager attributes = affectedObject.GetComponent<AttributeManager>();
        if (attributes.health > 0)
        {
            //attributes.TakeDamage(damagePerSecond);
        }
    }
}

public class Ice : StatusEffect
{
    private float speedMultiplier = 0.7f;

    public Ice() : base()
    {
    }

    public override void ApplyEffect()
    {
        active = true;
        AttributeManager attributes = affectedObject.GetComponent<AttributeManager>();
        if (attributes.health > 0)
        {
            attributes.SpeedModifier(speedMultiplier);
        }

        StartCoroutine(IceCoroutine());
    }

    public void ApplyEffect(float duration)
    {
        active = true;
        this.duration = duration;
        AttributeManager attributes = affectedObject.GetComponent<AttributeManager>();
        if (attributes.health > 0)
        {
            attributes.SpeedModifier(speedMultiplier);
        }

        StartCoroutine(IceCoroutine());
    }

    public override void RemoveEffect()
    {
        active = false;
        AttributeManager attributes = affectedObject.GetComponent<AttributeManager>();
        if (attributes.health > 0)
        {
            attributes.SpeedModifier(1 / speedMultiplier);
        }
    }

    public override string GetStatusType()
    {
        return "ice";
    }

    private IEnumerator IceCoroutine()
    {


        yield return new WaitForSeconds(duration);

        RemoveEffect();
    }
}