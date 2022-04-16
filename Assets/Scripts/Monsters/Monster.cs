using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    CHAIR,
    BOOKCASE,
    GHOST,
    WARDROBE,
    PLANT,
}

public class Monster : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected MonsterType _monsterType;
    [SerializeField] protected GameObject _furnitureObject;
    [SerializeField] protected GameObject _monsterObject;
    [SerializeField] protected bool _rememberLastPosition;

    protected bool _isMonster;

    private void OnEnable()
    {
        LightsManager.OnLightsSwitched += TransformObject;
    }

    private void OnDisable()
    {
        LightsManager.OnLightsSwitched -= TransformObject;
    }

    public virtual void TransformObject(bool furniture)
    {
        if(furniture)
        {
            HideMonster();
        }
        else
        {
            ShowMonster();
        }
    }

    public void ShowMonster()
    {
        _furnitureObject.SetActive(false);
        _monsterObject.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("Default");
        _isMonster = true;
    }

    public void HideMonster()
    {
        _furnitureObject.SetActive(true);
        _monsterObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        _isMonster = false;
        if (_monsterType != MonsterType.GHOST)
            _monsterObject.transform.localPosition = Vector2.zero;
    }
}
