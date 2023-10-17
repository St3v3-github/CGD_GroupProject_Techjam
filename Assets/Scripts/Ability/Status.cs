using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    protected float duration;
    protected GameObject affectedObject;


    public StatusEffect(float duration, GameObject affectedObject)
    {
        this.duration = duration;
        this.affectedObject = affectedObject;
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}

public class Fire : StatusEffect
{
    private float damagePerSecond = 4;

    public Fire(float duration, float damagePerSecond, GameObject affectedObject) : base(duration, affectedObject)
    {
        this.damagePerSecond = damagePerSecond;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        StartCoroutine(FireCoroutine());
    }

    public override void RemoveEffect()
    {
        CancelInvoke("DealDamage");
    }

    private IEnumerator FireCoroutine()
    {
        InvokeRepeating("DealDamage", 1f, 1f); 

        yield return new WaitForSeconds(duration);

        RemoveEffect();
    }

    private void DealDamage()
    {
        PlayerController player = affectedObject.GetComponent<PlayerController>();
        if (player.health > 0)
        {
            player.TakeDamage(damagePerSecond);
        }
    }
}

public class Ice : StatusEffect
{
    private float speedMultiplier = 0.7f;

    public Ice(float duration, float speedMultiplier, GameObject affectedObject) : base(duration, affectedObject)
    {
        this.speedMultiplier = speedMultiplier;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        PlayerController player = affectedObject.GetComponent<PlayerController>();

        player.Speedmodifier(speedMultiplier);

        StartCoroutine(IceCoroutine());

        
    }

    public override void RemoveEffect()
    {
        PlayerController player = affectedObject.GetComponent<PlayerController>();

        player.Speedmodifier( (1/speedMultiplier));
    }

    private IEnumerator IceCoroutine()
    {
        

        yield return new WaitForSeconds(duration);

        RemoveEffect();
    }

}

