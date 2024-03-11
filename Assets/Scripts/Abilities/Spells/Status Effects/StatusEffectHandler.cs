using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour, IEffectable
{
    private StatusEffect_Data _data;
    public UpdatedPlayerController selfMovement;
    public AttributeManager selfAttributes;

    private GameObject effectParticles;

    private void Start()
    {
        selfMovement = GetComponent<UpdatedPlayerController>();
        selfAttributes = GetComponent<AttributeManager>();
    }

    private void Update()
    {
        if (_data != null & selfAttributes != null) HandleEffect();
    }

    public void ApplyEffect(StatusEffect_Data _data)
    {
        RemoveEffect();
        this._data = _data;
        Debug.Log("status effect? > " + _data);
        Debug.Log(_data); 
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
        /*if (_testDummy.getCurrentMoveSpeed() != _testDummy.getBaseMoveSpeed())
        {
            _testDummy.setNewMoveSpeed(_testDummy.getBaseMoveSpeed());
        }*/

        // Add slow for PvP (can get current speed but not base speed ?)
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

            /*dummy test
            float _currentHealth = _testDummy.getCurrentHealth();
            float _newHealth = _currentHealth - _data.DOT_Amount;


            Debug.Log($"currentEffectTime: {currentEffectTime}, lastTickTime: {nextTickTime}, TickSpeed: {_data.TickSpeed}");
            Debug.Log($"Condition: {currentEffectTime > nextTickTime + _data.TickSpeed}");
            Debug.Log($"New Health: {_newHealth}");

            _currentHealth = Mathf.Clamp(_currentHealth, 0, _testDummy.getMaxHealth());
            _testDummy.setCurrentHealth(_newHealth);*/


            //PvP test
            float currentHealth = selfAttributes.GetPlayerHealth();
            float newHealth = currentHealth - _data.DOT_Amount;

            currentHealth = Mathf.Clamp(currentHealth, 0, selfAttributes.GetMaxHealth());
            selfAttributes.SetPlayerHealth(newHealth);
        }
        if(_data.MovementPen > 0)
        {
            //edit for PvP - same as above
            nextTickTime += _data.TickSpeed;
            /*float newMoveSpeed = (_testDummy.getBaseMoveSpeed() / _data.MovementPen);
            _testDummy.setNewMoveSpeed(newMoveSpeed);
            Debug.Log(_testDummy.getCurrentMoveSpeed());*/
        }
    }
}
