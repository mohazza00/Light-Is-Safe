using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private PlayerController _target;
    [SerializeField] private float _smoothTime;

    [Header("Showing The Level")]
    [HideInInspector] public bool IsShowingLevel;
    [SerializeField] private Transform _door;
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _waitingTime;

   

    [Header("Dependencies")]
    private GameManager _gameManager;
    private Camera _cam;

    private float _waitingTimeCounter;
    private bool _moveTowardsTheDoor = true;
    private Vector3 _vel;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;

        Vector3 targetPosition = _target.transform.position;
        targetPosition.z = -10f;
        transform.position = targetPosition;
        _waitingTimeCounter = _waitingTime;
    }

    private void Update()
    {
        if(_gameManager.CurrentGameState == GameState.SHOWING_THE_LEVEL)
        {
            if(IsShowingLevel)
            {
                if(_moveTowardsTheDoor)
                {
                    Vector3 targetPosition = Vector2.MoveTowards(transform.position, _door.position, _movingSpeed * Time.deltaTime);
                    targetPosition.z = -10f;

                    transform.position = targetPosition;

                    if (Vector2.Distance(transform.position, _door.position) < 0.2f)
                    {
                        if (_waitingTimeCounter <= 0)
                        {
                            _waitingTimeCounter = _waitingTime;
                            _moveTowardsTheDoor = false;
                        }
                        else
                        {
                            _waitingTimeCounter -= Time.deltaTime;
                        }
                    }
                }
                else
                {
                    Vector3 targetPosition = Vector2.MoveTowards(transform.position, _target.transform.position, _movingSpeed * Time.deltaTime);
                    targetPosition.z = -10f;

                    transform.position = targetPosition;

                    if (Vector2.Distance(transform.position, _target.transform.position) < 0.05f)
                    {
                            IsShowingLevel = false;
                    }
                }
            
            }
        }
        
        if(!IsShowingLevel)
        {
            Vector3 targetPosition = _target.transform.position;

            targetPosition.z = -10f;

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _vel , _smoothTime * Time.deltaTime);
            transform.position = smoothedPosition;
        }
       
    }
}
