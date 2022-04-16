using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lock Box Object Data")]
public class LockBoxObject : ScriptableObject
{
    public GameObject Prefab;
    public Sprite Icon;
    public int Amount;
}
