using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Monster
{
    [Header("Type Settings")]
    public Transform[] MoveSpots;
    public float Speed;
    public float WaitingTime;

    private float _waitingTimeCounter;
    private int _currentSpotIndex;

    private void Start()
    {
        _waitingTimeCounter = WaitingTime;
    }

    private void Update()
    {
        if (!_isMonster)
            return;

        _monsterObject.transform.position = Vector2.MoveTowards(_monsterObject.transform.position, MoveSpots[_currentSpotIndex].position, Speed * Time.deltaTime);
        
        if(Vector2.Distance(_monsterObject.transform.position, MoveSpots[_currentSpotIndex].position) < 0.2f)
        {
            if(_waitingTimeCounter <= 0)
            {
                _waitingTimeCounter = WaitingTime;
                _currentSpotIndex = (_currentSpotIndex + 1) % MoveSpots.Length;
            }
            else
            {
                _waitingTimeCounter -= Time.deltaTime;
            }

        }
    }

    public override void TransformObject(bool on)
    {
        base.TransformObject(on);
        if (on)
        {
            _currentSpotIndex = 0;
        }
        
    }
}
