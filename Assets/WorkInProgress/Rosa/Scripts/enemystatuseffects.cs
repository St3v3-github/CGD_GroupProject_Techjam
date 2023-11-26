using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemystatuseffects : MonoBehaviour, IEffectable
{
    private StatusEffect_Data _data;
    public _testDummy _testDummy;

    private GameObject effectParticles;

    private void Update()
    {
        if (_data != null & _testDummy != null) HandleEffect();
    }

    public void ApplyEffect(StatusEffect_Data _data)
    {
        RemoveEffect();
        this._data = _data;
        effectParticles = Instantiate(_data.EffectParticles, transform);
    }

    private float currentEffectTime = 0f;
    private float nextTickTime = 0f;

    public void RemoveEffect()
    {
        _data = null;
        currentEffectTime = 0f;
        nextTickTime = 0f;

        if(effectParticles != null)
        {
            Destroy(effectParticles);
        }
        if(_testDummy.getCurrentMoveSpeed() != _testDummy.getBaseMoveSpeed())
        {
            _testDummy.setNewMoveSpeed(_testDummy.getBaseMoveSpeed());
        }
    }

    
    public void HandleEffect()
    {
        currentEffectTime += Time.deltaTime;
        if(currentEffectTime >= _data.Lifetime)
        {
            RemoveEffect();
        }

        if(_data == null) { return; }
        if (_data.DOT_Amount != 0 && currentEffectTime > nextTickTime)
        {
            nextTickTime += _data.TickSpeed;

            float currentHealth = _testDummy.getCurrentHealth();
            float newHealth = currentHealth - _data.DOT_Amount;

            Debug.Log($"currentEffectTime: {currentEffectTime}, lastTickTime: {nextTickTime}, TickSpeed: {_data.TickSpeed}");
            Debug.Log($"Condition: {currentEffectTime > nextTickTime + _data.TickSpeed}");
            Debug.Log($"New Health: {newHealth}");

            currentHealth = Mathf.Clamp(currentHealth, 0, _testDummy.getMaxHealth());
            _testDummy.setCurrentHealth(newHealth);
        }
        if(_data.MovementPen > 0)
        {
            nextTickTime += _data.TickSpeed;
            float newMoveSpeed = (_testDummy.getBaseMoveSpeed() / _data.MovementPen);
            _testDummy.setNewMoveSpeed(newMoveSpeed);
            Debug.Log(_testDummy.getCurrentMoveSpeed());
        }
    }
}
