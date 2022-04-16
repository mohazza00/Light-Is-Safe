using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        CLICK,
        CARRY,
        HOLD,
        LOCK_BOX,
    }

    public InteractionType InteractionWay;
    public Material DefaultMaterial;
    public Material OutlineMaterial;
    public SpriteRenderer sprite;
    public ObjectType ObjectType;

    public float LookPercentage;
    public bool CanInteract = true;

    protected virtual void Start()
    {
       
    }

    public void Select()
    {
        sprite.material = OutlineMaterial;
    }

    public void Deselect()
    {
        sprite.material = DefaultMaterial;
    }

    public void UseObject()
    {
        Destroy(gameObject);
    }

}
