using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;



public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Camera cam;
    

    [Space]
    [SerializeField] private CharacterGrowth characterGrowth;
    [SerializeField] private CharacterAnimationMonitor animationMonitor;

    public CharacterConfig CharacterConfig { get; set; }


    private void Awake()
    {
        characterGrowth.OnGrowthStageUpdated += OnGrowthStageUpdated;

    }

    private void OnGrowthStageUpdated(GrowthStageData growthStageData)
    {
        CharacterConfig characterConfig = new CharacterConfig(growthStageData);

        // Update Sprites
        animationMonitor.UpdateSprites(characterConfig.sprites);

        // Update Speed
        // Update Scale
        // Update Camera Zoom
    }

    private void SetCharacterScale(float scale)
    {
        // placeholder implementation
        characterTransform.localScale = Vector3.one * scale;
    }

    private void SetCharacterSpeed(float speed)
    {
        // implementation
    }

    private void SetCameraZoom(float zoom)
    {
        // implementation
    }
}


public class CharacterConfig
{
    public SerializedDictionary<Directions, Sprite> sprites = new();
    public float speed = 1f;
    public float manueverability = 1f;
    public float scale = 1f;
    public float cameraZoom = 10f;

    public CharacterConfig(GrowthStageData growthStageData)
    {
        this.sprites = growthStageData.sprites;
        this.speed = growthStageData.speed;
        this.manueverability = growthStageData.manueverability;
        this.scale = growthStageData.scale;
        this.cameraZoom = growthStageData.cameraZoom;
    }
}
