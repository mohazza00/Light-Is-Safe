using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    public SpriteRenderer ClothesSprite;

    private float _waitingTimeCounter;
    private int _currentSpotIndex;

    [Header("Movement")]
    public float Frequency = 5f;
    public float Magnitude = 0.5f;
    public float Speed = 2;
    private bool _startMoving;
    private Vector2 _pos;
    private Vector2 _direction;
    private Vector2 _startPosition;

    private PoolObject _poolObject;
    private float _timer;
    private bool _canCheck;
    

    private void OnEnable()
    {
        LightsManager.OnLightsSwitched += DiableClothes;
        _canCheck = false;
        _timer = 0f;
    }

    private void OnDisable()
    {
        LightsManager.OnLightsSwitched -= DiableClothes;
    }

    private void Awake()
    {
        ClothesSprite = GetComponent<SpriteRenderer>();
        _poolObject = GetComponent<PoolObject>();
        _startPosition = transform.position;
    }

    public void ResetPosition()
    {
        _pos = transform.position;
    }
    private void Update()
    {
        if (!_startMoving)
            return;

        _pos += _direction * Time.deltaTime * Speed;
        transform.position = _pos + Vector2.Perpendicular(_direction) * Mathf.Sin(Time.time * Frequency) * Magnitude;

        if (_canCheck)
            return;

        if (_timer >= 0.3f)
        {
            _canCheck = true;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public void StartMoving(Vector2 dir)
    {
        _direction = dir;
        _startMoving = true;
    }

    private void DiableClothes(bool on)
    {
        if (on)
        {
            _poolObject.TurnOff();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canCheck)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            PoolingManager.Instance.SpawnObj(PoolObjectType.SMOKE, transform.position, null);
            _poolObject.TurnOff();
        }
    }
}
