using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrderManager : MonoBehaviour
{
    private SpriteSortOrderController[] sortOrderControllers;

    private void Awake()
    {
        sortOrderControllers = GameObject.FindObjectsOfType<SpriteSortOrderController>();
    }

    private void Update()
    {
        foreach(SpriteSortOrderController controller in sortOrderControllers)
        {
            controller.RunUpdate();
        }
    }
}
