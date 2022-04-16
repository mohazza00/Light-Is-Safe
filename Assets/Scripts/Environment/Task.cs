using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public ObjectType TaskObjectType;

    [Header("Settings")]
    [SerializeField] private ObjectData _objectData;
    [SerializeField] private GameObject _rewardObject;
    [SerializeField] private bool _isFinalTask;

    [Header("Components")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _objectSprite;
    [SerializeField] private Transform _rewardSpawningSpot;
    [SerializeField] private GameObject _visibleRewardObject;

    private Animator _animator;
    private bool _taskActive = true;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rewardSpawningSpot = transform.Find("RewardSpot");
        _visibleRewardObject = transform.Find("VisibleReward").gameObject;
    }

    private void Start()
    {
        if (_objectData == null || _objectSprite == null)
            return;
        _objectSprite.sprite = _objectData.Icon;
        TaskObjectType = _objectData.Type;
    }

    public virtual void ClearTask()
    {
        _animator.SetBool("Open", false);
        PoolingManager.Instance.SpawnObj(PoolObjectType.STARS, _rewardSpawningSpot.position, null);
        _taskActive = false;
        _collider.enabled = false;
        _visibleRewardObject.SetActive(false);

        if (_isFinalTask)
        {
            GameManager.Instance.ClearLevel();
            return;
        }
        else
        {
            AudioManager.Instance.PlaySound(SoundName.TASK_CLEAR);
        }

        if (_rewardObject != null)
            Instantiate(_rewardObject, _rewardSpawningSpot.position, Quaternion.identity);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_taskActive)
            return;

        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Player.PlayerInteraction.CurrentTask = this;
            _animator.SetBool("Open", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Player.PlayerInteraction.CurrentTask = null;
            _animator.SetBool("Open", false);
        }
    }
}
