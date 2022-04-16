using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public float RotateSpeed;
    public SpriteRenderer BookSprite;

    private PoolObject _poolObject;

    private float _timer;
    private bool _canCheck;

    private void OnEnable()
    {
        LightsManager.OnLightsSwitched += DiableBook;
        _canCheck = false;
        _timer = 0f;
    }

    private void OnDisable()
    {
        LightsManager.OnLightsSwitched -= DiableBook;
    }

    private void Awake()
    {
        BookSprite = GetComponent<SpriteRenderer>();
        _poolObject = GetComponent<PoolObject>();
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);

        if (_canCheck)
            return;

        if(_timer >= 0.3f)
        {
            _canCheck = true;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void DiableBook(bool on)
    {
        if(on)
        {
            _poolObject.TurnOff();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canCheck)
            return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            PoolingManager.Instance.SpawnObj(PoolObjectType.SMOKE, transform.position, null);
            _poolObject.TurnOff();
        }
    }
}
