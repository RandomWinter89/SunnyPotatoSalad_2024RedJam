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
    //public SerializedDictionary<Directions, Sprite> sprites = new()
    //{
    //    { Directions.Up, null},
    //    { Directions.Down, null},
    //    { Directions.Left, null},
    //    { Directions.Right, null},
    //};

    /// <summary>
    /// Defines the series of sprites to be used for a direction
    /// </summary>
    public SerializedDictionary<Directions, SpriteSeries> sprites = new()
    {
        {Directions.Up, null },
        {Directions.Down, null },
        {Directions.Left, null },
        {Directions.Right, null },
    };


    public float speed = 1f;
    public float manueverability = 1f;
    public float scale = 1f;
    public float cameraZoom;
}

public enum Directions
{
    Up,
    Down,
    Left,
    Right
}

[System.Serializable]
public class SpriteSeries
{
    public List<Sprite> series = new List<Sprite>();
    public float framesPerSecond = 60;
    public float speed = 1f;
    public bool loop = true;
    public bool flip = false;

    public Sprite FirstSprite => series[0];
    public Sprite LastSprite => series[series.Count - 1];
    public Sprite CurrentSprite => series[_currentIndex];



    private float _nextFrameTime = 0;
    private int _currentIndex = 0;


    private void TraverseSeries()
    {
        _currentIndex++;

        if (!loop) return;
        if (_currentIndex > series.Count - 1)
        {
            _currentIndex = 0;
        }
    }

    public void Enter()
    {
        _currentIndex = 0;
        _nextFrameTime = (Time.time + (1 / framesPerSecond * Time.deltaTime)) * speed;
    }

    public void Update()
    {
        if(Time.time > _nextFrameTime)
        {
            TraverseSeries();

            _nextFrameTime = (Time.time + (1 / framesPerSecond * Time.deltaTime)) * speed;
        }
    }
}