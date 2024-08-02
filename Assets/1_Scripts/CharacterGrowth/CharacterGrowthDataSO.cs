using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;



[CreateAssetMenu(fileName = "CharacterGrowthDataSO", menuName = "Custom/CharacterGrowthDataSO")]
public class CharacterGrowthDataSO : ScriptableObject
{
    public SerializedDictionary<int, GrowthStageData> growthStageDatas;
}


[System.Serializable]
public class GrowthStageData
{
    // include directions
    public SerializedDictionary<Directions, Sprite> sprites = new()
    {
        { Directions.Up, null},
        { Directions.Down, null},
        { Directions.Left, null},
        { Directions.Right, null},
    };

    public float speed = 1f;
    public float manueverability = 1f;
}

public enum Directions
{
    Up,
    Down,
    Left,
    Right
}