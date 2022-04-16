using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Interactable
{
    public bool BeignPushed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void StartPushing()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        sprite.sortingLayerName = "Player";

    }

    public void StopPushing()
    {
        _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        sprite.sortingLayerName = "Default";

    }
}
