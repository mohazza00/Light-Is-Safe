using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    NONE,
    STARS,
    BOOK_ENEMY,
    HEALING,
    SMOKE,
    CLOTHES,
}

public class PoolObjectLoader : MonoBehaviour
{
    public static PoolObject InstantiatePrefab(PoolObjectType objType)
    {
        GameObject obj = null;

        switch (objType)
        {
            //case PoolObjectType.GOLDEN_KEY:
            //    {
            //        obj = Instantiate(Resources.Load("GoldenKey", typeof(GameObject)) as GameObject);
            //        break;
            //    }

            case PoolObjectType.STARS:
                {
                    obj = Instantiate(Resources.Load("Stars", typeof(GameObject)) as GameObject);
                    break;
                }

            case PoolObjectType.BOOK_ENEMY:
                {
                    obj = Instantiate(Resources.Load("Book", typeof(GameObject)) as GameObject);
                    break;
                }

            case PoolObjectType.HEALING:
                {
                    obj = Instantiate(Resources.Load("Healing", typeof(GameObject)) as GameObject);
                    break;
                }

            case PoolObjectType.SMOKE:
                {
                    obj = Instantiate(Resources.Load("Smoke", typeof(GameObject)) as GameObject);
                    break;
                }

            case PoolObjectType.CLOTHES:
                {
                    obj = Instantiate(Resources.Load("Clothes", typeof(GameObject)) as GameObject);
                    break;
                }

        }

        return obj.GetComponent<PoolObject>();
    }
}