using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthStage
{
    public int stage;

    // include directions
    public List<Sprite> sprites = new();

    public float speed = 1f;
}

public class CharacterGrowth : MonoBehaviour
{
    public GrowthStage growthStage;
    public float growthPercentage;

    public void IncreaseGrowth(float amount)
    {
        growthPercentage += amount;
        growthPercentage = Mathf.Clamp01(growthPercentage);

        if (growthPercentage >= 1)
        {
            growthStage.stage++;
            UpdateGrowth(growthStage.stage);
        }
    }

    private void UpdateGrowth(int growthStage)
    {
        // Update Asset
        // Update Speed
        // Update Scale
        // Update Camera Zoom
    }
}
