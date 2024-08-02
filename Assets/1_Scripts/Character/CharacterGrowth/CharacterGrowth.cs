using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterGrowth : MonoBehaviour
{
    [SerializeField] CharacterGrowthDataSO characterGrowthDataSO;
    [SerializeField, NaughtyAttributes.ReadOnly] private int growthStage = 0;
    [SerializeField, NaughtyAttributes.ReadOnly, NaughtyAttributes.ProgressBar(1f)] private float growthPercentage;
    private List<CharacterGrowthItem> collectedGrowthItems = new();

    public System.Action<GrowthStageData> OnGrowthStageUpdated;


    public GrowthStageData CurrentGrowthStageData => characterGrowthDataSO.growthStageDatas[growthStage];


    private void OnDestroy()
    {
        OnGrowthStageUpdated = null;
    }


    public void IncreaseGrowth(CharacterGrowthItem growthItem)
    {
        growthPercentage += growthItem.GrowthAmount;
        growthPercentage = Mathf.Clamp01(growthPercentage);

        if (growthPercentage >= 1)
        {
            growthStage++;
            UpdateGrowth(growthStage);
        }

        // for use when character touched a border an drops the items
        collectedGrowthItems.Add(growthItem);
    }

    private void UpdateGrowth(int growthStage)
    {
        GrowthStageData data = characterGrowthDataSO.growthStageDatas[growthStage];
        OnGrowthStageUpdated?.Invoke(data);
    }

    #region For Debugging
    private void SetGrowthStage(int growthStage)
    {
        this.growthStage = growthStage;
        UpdateGrowth(growthStage);
    }
    #endregion
}
