using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour, IEffectable
{
    public StatusEffect_Data _data;
    public UpdatedPlayerController selfMovement;
    public AttributeManager selfAttributes;
    public FullScreenFXHandler fxHandler;

    private GameObject effectParticles;

    private void Start()
    {
      //  selfMovement = GetComponent<UpdatedPlayerController>();
       // selfAttributes = GetComponent<AttributeManager>();
    }

    private void Update()
    {
        if (_data != null & selfAttributes != null) HandleEffect();
    }

    public void ApplyEffect(StatusEffect_Data _data)
    {
        Debug.Log("Applying Status");
        RemoveEffect();
        this._data = _data;
        Debug.Log("status effect? > " + _data);
        Debug.Log(_data); 
        effectParticles = Instantiate(_data.EffectParticles, transform);
        nextTickTime = _data.TickSpeed;

        if (_data.isFire)
        {
            fxHandler.ToggleFireOn();
        }
        else if (_data.isIce)
        {
            fxHandler.ToggleIceOn();
        }
        else if (_data.isLightning)
        {
            fxHandler.ToggleLightningOn();
        }
        else if (_data.isWind)
        {
            fxHandler.ToggleWindOn();
        }
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
            fxHandler.ToggleEffectsOff();
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
            if(nextTickTime < _data.TickSpeed)
            {
                nextTickTime += Time.deltaTime;
            }
            else if(nextTickTime >= _data.TickSpeed)
            {
                selfAttributes.TakeDamage(_data.DOT_Amount);
                nextTickTime = 0;
            }
            

            //Increase Next tick by delta
            // when next tick > tick speed
            //DO DAMAGE -> next tick to 0

            /*dummy test
            float _currentHealth = _testDummy.getCurrentHealth();
            float _newHealth = _currentHealth - _data.DOT_Amount;


            Debug.Log($"currentEffectTime: {currentEffectTime}, lastTickTime: {nextTickTime}, TickSpeed: {_data.TickSpeed}");
            Debug.Log($"Condition: {currentEffectTime > nextTickTime + _data.TickSpeed}");
            Debug.Log($"New Health: {_newHealth}");

            _currentHealth = Mathf.Clamp(_currentHealth, 0, _testDummy.getMaxHealth());
            _testDummy.setCurrentHealth(_newHealth);*/


            //PvP test
            //float currentHealth = selfAttributes.GetPlayerHealth();
           
          //  float newHealth = currentHealth - _data.DOT_Amount;

        //currentHealth = Mathf.Clamp(currentHealth, 0, selfAttributes.GetMaxHealth());
            //selfAttributes.SetPlayerHealth(newHealth);
        }
        if(_data.MovementPen > 0 && currentEffectTime > nextTickTime)
        {
            if(nextTickTime < _data.TickSpeed)
            {
                nextTickTime += Time.deltaTime;
            }
            else if(nextTickTime >= _data.TickSpeed)
            {
                selfMovement.speedMultiplier = _data.MovementPen;
                nextTickTime = 0;
            }
            //edit for PvP - same as above
           // nextTickTime += _data.TickSpeed;
            /*float newMoveSpeed = (_testDummy.getBaseMoveSpeed() / _data.MovementPen);
            _testDummy.setNewMoveSpeed(newMoveSpeed);
            Debug.Log(_testDummy.getCurrentMoveSpeed());*/
        }
    }
}
