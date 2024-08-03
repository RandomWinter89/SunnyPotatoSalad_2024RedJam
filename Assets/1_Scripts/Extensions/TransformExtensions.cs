using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyAllChilds(this Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {

#if UNITY_EDITOR

            if (Application.isPlaying)
                GameObject.Destroy(parent.GetChild(i).gameObject);
            else
                GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
#else
            GameObject.Destroy(parent.GetChild(i).gameObject);
#endif
        }
    }
}
