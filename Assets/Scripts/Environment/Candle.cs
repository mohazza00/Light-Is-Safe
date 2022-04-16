using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Candle : Pickable
{
    public GameObject CandleLightObj;
    public Light2D CandleLight;
    public float Lifetime;
    public bool IsLit;

    private float _currentLife;

    public PlayerController Player;

    protected override void Start()
    {
        base.Start();
        _currentLife = Lifetime;
    }

    public override void PickUpObject(Transform carryPoint)
    {
        base.PickUpObject(carryPoint);
        
    }

    public override void DropObject(Vector3 dropSpot)
    {
        base.DropObject(dropSpot);
        if(IsLit)
        {
            CandleLightObj.SetActive(false);
            IsLit = false;
        }
   
    }

    public void SetPlayer(PlayerController player)
    {
        Player = player;
    }

    public void UnsetPlayer()
    {
        Player = null;
    }

    private void Update()
    {
        if(IsLit)
        {
            _currentLife -= Time.deltaTime;

            if(_currentLife <= 7f)
            {
                CandleLight.intensity = 0.7f;
            }

            if (_currentLife <= 3f)
            {
                CandleLight.intensity = 0.4f;
            }
            if (_currentLife <= 0f)
            {
                if (Player != null)
                    Player.PlayerInteraction.CandleDead();
                Destroy(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        LightsManager.OnLightsSwitched += HandleCandleState;
    }

    private void OnDisable()
    { 
        LightsManager.OnLightsSwitched -= HandleCandleState;
    }

    private void HandleCandleState(bool on)
    {
        if(on)
        {
            CandleLightObj.SetActive(false);
            IsLit = false;
        }
        else
        {
            if (Player == null)
                return;

            CandleLightObj.SetActive(true);
            IsLit = true;
        }
    }

}
