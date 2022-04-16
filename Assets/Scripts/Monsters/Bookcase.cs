using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookcase : Monster
{
    [Header("Settings")]
    public Sprite[] Books;
    public Transform ThrowingPoint;
    public Transform[] ThrowingTargets;
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
        if(_isMonster)
        {
            if(_throwingTimer <= 0)
            {
                ThrowBook();
                Debug.Log("Shoot");
                _throwingTimer = ThrowingRate;
            }
            else
            {
                _throwingTimer -= Time.deltaTime;
            }
        }
    }


   
    private void ThrowBook()
    {
        int rand = Random.Range(0, Books.Length);
        GameObject obj = PoolingManager.Instance.GetObject(PoolObjectType.BOOK_ENEMY);
        if(obj != null)
        {
            Book book = obj.GetComponent<Book>();
            book.BookSprite.sprite = Books[rand];
            obj.transform.position = ThrowingPoint.position;
            obj.transform.rotation = Quaternion.identity;

            obj.SetActive(true);

            Vector2 dir = (ThrowingTargets[_currentSpotIndex].position - ThrowingPoint.position).normalized;
            book.GetComponent<Rigidbody2D>().AddForce(dir * ThrowingSpeed);

            _currentSpotIndex = (_currentSpotIndex + 1) % ThrowingTargets.Length;
        }
    }

   
}
