using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterGrowth : MonoBehaviour
{
    [SerializeField] CharacterGrowthDataSO characterGrowthDataSO;
    [SerializeField, NaughtyAttributes.ReadOnly] private int growthStage = 0;
    [SerializeField, NaughtyAttributes.ReadOnly, NaughtyAttributes.ProgressBar(1f)] private float growthPercentage;
    [SerializeField, NaughtyAttributes.ReadOnly] private List<CharacterGrowthItem> collectedGrowthItems = new();

    public System.Action<GrowthStageData> OnGrowthStageUpdated;


    public GrowthStageData FirstGrowthStageData => characterGrowthDataSO.growthStageDatas[0];
    public GrowthStageData LastGrowthStageData => characterGrowthDataSO.growthStageDatas[characterGrowthDataSO.growthStageDatas.Count - 1];
    public GrowthStageData CurrentGrowthStageData => characterGrowthDataSO.growthStageDatas[growthStage];

    [Header("Debug")]
    [SerializeField] private int startingGrowthStage = 0;


    private void OnDestroy()
    {
        OnGrowthStageUpdated = null;
    }

    private void Start()
    {
        SetGrowthStage(startingGrowthStage);
    }


    public void IncreaseGrowth(CharacterGrowthItem growthItem)
    {
        growthPercentage += growthItem.GrowthAmount;
        growthPercentage = Mathf.Clamp01(growthPercentage);

        if (growthPercentage >= 1)
        {
            growthStage++;
            UpdateGrowth(growthStage);
            growthPercentage = 0f;
        }

        // for use when character touched a border an drops the items
        collectedGrowthItems.Add(growthItem);
    }

    #region Decrease Growth
    [NaughtyAttributes.Button]
    private void TestDrop()
    {
        DecreaseGrowth(.5f);
    }

    public void DecreaseGrowth(float amount)
    {
        growthPercentage -= amount;
        growthPercentage = Mathf.Clamp01(growthPercentage);

        // drop items
        for(int i = 0; i < 3; i++)
        {
            if(TryGetRandomGrowthItemInList(out CharacterGrowthItem growthItem))
            {
                Drop(growthItem);
            }
        }
    }

    private void Drop(CharacterGrowthItem growthItem)
    {
        growthItem.Drop(transform.position);
        collectedGrowthItems.Remove(growthItem);
    }

    private bool TryGetRandomGrowthItemInList(out CharacterGrowthItem growthItem)
    {
        if(collectedGrowthItems.Count == 0)
        {
            growthItem = null;
            return false;
        }

        int index = Random.Range(0, collectedGrowthItems.Count);
        growthItem = collectedGrowthItems[index];
        return true;
    }


    #endregion

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
