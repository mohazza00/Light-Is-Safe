using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : Monster
{
    [Header("Settings")]
    public Sprite[] Clothes;
    public Transform ThrowingPoint;
    public float ThrowingRate;
    public float ThrowingSpeed;

    private float _throwingTimer;
    private int _currentSpotIndex;


    private void Start()
    {
        _throwingTimer = ThrowingRate;
    }

    private void Update()
    {
        if (_isMonster)
        {
            if (_throwingTimer <= 0)
            {
                SummonClothes();
                Debug.Log("Shoot");
                _throwingTimer = ThrowingRate;
            }
            else
            {
                _throwingTimer -= Time.deltaTime;
            }
        }
    }

    private void SummonClothes()
    {
        int rand = Random.Range(0, Clothes.Length);
        GameObject obj = PoolingManager.Instance.GetObject(PoolObjectType.CLOTHES);
        if (obj != null)
        {
            Clothes clothes = obj.GetComponent<Clothes>();
            clothes.ClothesSprite.sprite = Clothes[rand];
            obj.transform.position = ThrowingPoint.position;
            obj.transform.rotation = Quaternion.identity;
            clothes.ResetPosition();

            obj.SetActive(true);

            Vector2 dir = (GameManager.Instance.Player.transform.position - ThrowingPoint.position).normalized;
            clothes.StartMoving(dir);

        }
    }
}
