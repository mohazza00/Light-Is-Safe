using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactable
{
    public virtual void PickUpObject(Transform carryPoint)
    {
        transform.parent = carryPoint;
        transform.localPosition = Vector3.zero;
        sprite.sortingLayerName = "Player";
    }

    public virtual void DropObject(Vector3 dropSpot)
    {
        transform.parent = null;
        transform.position = dropSpot;
        sprite.sortingLayerName = "Default";
    }

   
}
