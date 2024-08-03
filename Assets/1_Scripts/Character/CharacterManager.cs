using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;



public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Camera cam;


    [Space]
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterGrowth characterGrowth;
    [SerializeField] private CharacterAnimationMonitor animationMonitor;

    public CharacterConfig CharacterConfig { get; set; }


    private void Awake()
    {
        characterGrowth.OnGrowthStageUpdated += OnGrowthStageUpdated;
        OnGrowthStageUpdated(characterGrowth.FirstGrowthStageData);
    }

    private void Update()
    {
        animationMonitor.UpdateAnimation(characterMovement.Input);
    }

    private void OnGrowthStageUpdated(GrowthStageData growthStageData)
    {
        CharacterConfig characterConfig = new CharacterConfig(growthStageData);

        // Update Sprites
        animationMonitor.UpdateSprites(characterConfig.animationClipDatas);

        // Update Speed

        // Update Scale
        characterTransform.localScale = Vector3.one * characterConfig.scale;

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
    public SerializedDictionary<Directions, AnimationClipData> animationClipDatas = new();
    public float speed = 1f;
    public float manueverability = 1f;
    public float scale = 1f;
    public float cameraZoom = 10f;

    public CharacterConfig(GrowthStageData growthStageData)
    {
        this.animationClipDatas = growthStageData.animationClipDatas;
        this.speed = growthStageData.speed;
        this.manueverability = growthStageData.manueverability;
        this.scale = growthStageData.scale;
        this.cameraZoom = growthStageData.cameraZoom;
    }
}
