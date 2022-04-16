using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class Switch : MonoBehaviour
{
    [Header("Switch")]
    [SerializeField] private int _maxEnergyLevel = 3;
    [SerializeField] private LightSwitchData _switchData;
    [SerializeField] private SpriteRenderer _switchSprite;

    [Header("States")]
    public float CurrentEnergyIntensity;
    public float CurrentEnergyLifetime;
    public bool SwitchIsDead;
    public PlayerController TouchingPlayer;
    private int _currentEnergyLevel;

    [Header("Dependencies")]
    private GameManager _gameManager;

    private void Awake()
    {
        _switchSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _currentEnergyLevel = _maxEnergyLevel;
        _gameManager = GameManager.Instance;
        _switchSprite.color = _switchData.SwitchColor_High;
    }

    public void UseSwitch()
    {
        if (_currentEnergyLevel <= 0)
            return;

        GameManager.Instance.LightsManager.CurrentSwitch = this;

        if (_currentEnergyLevel == 3)
            SetSwitchDate(_switchData.Intensity_High, _switchData.LifeTime_High, _switchData.SwitchColor_Mid);
        
        else if (_currentEnergyLevel == 2)  
            SetSwitchDate(_switchData.Intensity_Mid, _switchData.LifeTime_Mid, _switchData.SwitchColor_Low);
        
        else if (_currentEnergyLevel == 1)
            SetSwitchDate(_switchData.Intensity_Low, _switchData.LifeTime_Low, _switchData.SwitchColor_Dead);
        
        _currentEnergyLevel--;

        if (_currentEnergyLevel <= 0) SwitchIsDead = true;     
    }

    private void SetSwitchDate(float intensity, float lifetime, Color switchColor)
    {
        CurrentEnergyIntensity = intensity;
        CurrentEnergyLifetime = lifetime;
        _switchSprite.color = switchColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SwitchIsDead || _gameManager.LightsManager.CurrentLightsState == LightsState.ON)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerInteraction>().HoldingSomething)
                return;

            TouchingPlayer = collision.GetComponent<PlayerController>();
            TouchingPlayer.IsTouchingSwitch = true;
            TouchingPlayer.CurrentSwitch = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(TouchingPlayer != null)
            {
                TouchingPlayer.IsTouchingSwitch = false;
                TouchingPlayer.CurrentSwitch = null;
                TouchingPlayer = null;
            }
            
        }
    }
}
