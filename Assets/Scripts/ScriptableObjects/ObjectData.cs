using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    GOLDEN_KEY,
    CHAIR,
    JUICE,
    CANDLE,
    SCISSORS,
    BOX,
    WOODEN_BOX,
    CROWBAR,
    FISH,
    WORKSHOP_KEY,
    BATTERY,
}

[CreateAssetMenu(menuName = "Data/Object Data")]
public class ObjectData : ScriptableObject
{
    public Sprite Icon;
    public ObjectType Type;
}
