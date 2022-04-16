using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum LightsState
{
    ON,
    OFF,
}

public class LightsManager : MonoBehaviour
{
    public delegate void LightsSwitch(bool on);
    public static event LightsSwitch OnLightsSwitched;

    [Header("Lights")]
    public Light2D GlobalLight;
    public Switch CurrentSwitch;
    public bool LightsOn;
    public float LightStartLifetime;

    private float _lightLifetime;
    private float _lightTimeCounter;

    private GameManager _gameManager;

    public LightsState CurrentLightsState;

    private Coroutine _turnOffLights;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    public void SetupLights()
    {
        GlobalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
        _lightLifetime = LightStartLifetime;
        _lightTimeCounter = _lightLifetime;
        LightsOn = true;
    }

    public void ResetGlobalLight()
    {
        StopCoroutine(_turnOffLights);
        GlobalLight.intensity = 1f;
        OnLightsSwitched?.Invoke(true);
        CurrentLightsState = LightsState.ON;
    }

    public void TurnOnLights()
    {
        AudioManager.Instance.PlaySound(SoundName.SWITCH);
        AudioManager.Instance.PlayNormalSound();

        CurrentSwitch.UseSwitch();
        OnLightsSwitched?.Invoke(true);

        _gameManager.Player.Torch.SetActive(false);
        GlobalLight.intensity = CurrentSwitch.CurrentEnergyIntensity;
        _lightLifetime = CurrentSwitch.CurrentEnergyLifetime;

        _lightTimeCounter = _lightLifetime;

        LightsOn = true;
        CurrentLightsState = LightsState.ON;
    }


    public void LightsCountDown()
    {
        if (_lightTimeCounter <= 0f)
        {
            _turnOffLights = StartCoroutine(TurnLightsOff());
            LightsOn = false;
            _lightTimeCounter = _lightLifetime;
        }
        else
        {
            _lightTimeCounter -= Time.deltaTime;
        }
    }

    private void TurnOffLights()
    {
        AudioManager.Instance.PlayScarySound();

        CurrentSwitch = null;

        _gameManager.Player.Torch.SetActive(true);
        GlobalLight.intensity = 0.03f;

        CurrentLightsState = LightsState.OFF;


        int rand = Random.Range(0, _gameManager.ScarySounds.Length);
        AudioManager.Instance.PlaySound(_gameManager.ScarySounds[rand]);

        OnLightsSwitched?.Invoke(false);
    }

    private IEnumerator TurnLightsOff()
    {
        AudioManager.Instance.PlaySound(SoundName.ZAPPING);

        for (int i = 0; i < 10; i++)
        {
            GlobalLight.intensity = 0.2f;
            yield return new WaitForSeconds(0.1f);
            GlobalLight.intensity = 1f;
            yield return new WaitForSeconds(0.1f);
            i++;
        }

        TurnOffLights();
    }
}
