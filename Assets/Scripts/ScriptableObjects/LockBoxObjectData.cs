using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Lock Box Object Data")]
public class LockBoxObjectData : ScriptableObject
{
    public GameObject Prefab;
    public Sprite Icon;
    public int Amount;
}
