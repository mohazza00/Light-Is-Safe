using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Task
{
    public GameObject DarkSpot;
    public override void ClearTask()
    {
        base.ClearTask();
        DarkSpot.SetActive(false);

    }
}
