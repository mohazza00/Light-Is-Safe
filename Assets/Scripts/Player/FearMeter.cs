using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FearMeter : MonoBehaviour
{
    public bool Debug;

    [Header("Fear")]
    public float MaxFear;
    public float CurrentFear;
    public bool Invincible;
    public float InvincibilityTime;

    public Image FearMeterUI;

    [Header("Hit Effect")]
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private Material _defaultMaterial;

    private bool _isDark;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        LightsManager.OnLightsSwitched += OnLightsSwitched;
    }

    private void OnDisable()
    {
        LightsManager.OnLightsSwitched -= OnLightsSwitched;
    }

    private void Update()
    {
        if (Debug)
            return;

        if (GameManager.Instance.CurrentGameState == GameState.GAMEOVER)
            return;

        if (GameManager.Instance.CurrentGameState == GameState.READY)
            return;


        if (_isDark)
        {
            if (_playerController.PlayerInteraction.HoldingCandle)
                return;

            IncreaseFearLevel();
        }
        else
        {
            DecreaseFearLevel();
        }
    }

    private void IncreaseFearLevel()
    {
        if(CurrentFear < MaxFear)
        {
            CurrentFear += Time.deltaTime;
            FearMeterUI.fillAmount = CurrentFear / MaxFear;

        }
        else
        {
            AudioManager.Instance.PlaySound(SoundName.HURT);

            Die();
        }
    }

    private void Die()
    {
        _playerController.ReleaseSoul();
    }

    private void DecreaseFearLevel()
    {
        if (CurrentFear > 0)
        {
            CurrentFear -= Time.deltaTime / 2f;
            FearMeterUI.fillAmount = CurrentFear / MaxFear;
        }
        
    }


    private void OnLightsSwitched(bool on)
    {
        _isDark = !on;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Invincible || GameManager.Instance.CurrentGameState == GameState.GAMEOVER)
            return;

        if(collision.gameObject.CompareTag("Monster"))
        {
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        AudioManager.Instance.PlaySound(SoundName.HURT);

        Invincible = true;
        _playerController.Sprite.material = _flashMaterial;
        CurrentFear += 2f;    
        CurrentFear += Time.deltaTime;
        FearMeterUI.fillAmount = CurrentFear / MaxFear;
      
        yield return new WaitForSeconds(0.1f);
        _playerController.Sprite.material = _defaultMaterial;

        if (CurrentFear >= MaxFear)
        {
            Die();
        }
        yield return new WaitForSeconds(InvincibilityTime);
        Invincible = false;
    }

    public void DrinkWater()
    {
        AudioManager.Instance.PlaySound(SoundName.DRINK);

        PoolingManager.Instance.SpawnObj(PoolObjectType.HEALING, Vector3.zero, transform);
        CurrentFear -= 5f;
        if (CurrentFear < 0f)
            CurrentFear = 0f;
        FearMeterUI.fillAmount = CurrentFear / MaxFear;

    }

}
