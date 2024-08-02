using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterGrowth : MonoBehaviour
{
    public int growthStage;
    public float growthPercentage;
    public CharacterGrowthDataSO characterGrowthDataSO;

    public void IncreaseGrowth(float amount)
    {
        growthPercentage += amount;
        growthPercentage = Mathf.Clamp01(growthPercentage);

        if (growthPercentage >= 1)
        {
            growthStage++;
            UpdateGrowth(growthStage);
        }
    }

    private void UpdateGrowth(int growthStage)
    {
        GrowthStageData data = characterGrowthDataSO.growthStageDatas[growthStage];

        // Update Asset
        // Update Speed
        // Update Scale
        // Update Camera Zoom


    }
}
