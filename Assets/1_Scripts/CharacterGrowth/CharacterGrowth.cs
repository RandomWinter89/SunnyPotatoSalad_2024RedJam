using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterGrowth : MonoBehaviour
{
    [SerializeField] CharacterGrowthDataSO characterGrowthDataSO;
    [SerializeField, NaughtyAttributes.ReadOnly] private int growthStage;
    [SerializeField, NaughtyAttributes.ReadOnly, NaughtyAttributes.ProgressBar(1f)] private float growthPercentage;


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

    private void SetGrowthStage(int growthStage)
    {
        this.growthStage = growthStage;
        UpdateGrowth(growthStage);
    }
}
