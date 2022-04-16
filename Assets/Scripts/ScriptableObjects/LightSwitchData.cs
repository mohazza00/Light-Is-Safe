using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Light Switch Data")]
public class LightSwitchData : ScriptableObject
{
    [Header("High Energy")]
    public float InnerRadius_High;
    public float OuterRadius_High;
    public float Intensity_High;
    public float LifeTime_High;
    public Color SwitchColor_High;

    [Header("Mid Energy")]
    public float InnerRadius_Mid;
    public float OuterRadius_Mid;
    public float Intensity_Mid;
    public float LifeTime_Mid;
    public Color SwitchColor_Mid;

    [Header("Low Energy")]
    public float InnerRadius_Low;
    public float OuterRadius_Low;
    public float Intensity_Low;
    public float LifeTime_Low;
    public Color SwitchColor_Low;

    [Header("Dead")]
    public Color SwitchColor_Dead;
}
