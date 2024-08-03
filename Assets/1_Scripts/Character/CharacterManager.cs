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

    public static CharacterManager Instance;

    public CharacterGrowth CharacterGrowth => characterGrowth;

    private void Awake()
    {
        characterGrowth.OnGrowthStageUpdated += OnGrowthStageUpdated;
        OnGrowthStageUpdated(characterGrowth.FirstGrowthStageData);

        Instance = this;
    }

    private void Update()
    {
        animationMonitor.UpdateAnimation(characterMovement.Velocity);
    }

    private void OnGrowthStageUpdated(GrowthStageData growthStageData)
    {
        CharacterConfig characterConfig = new CharacterConfig(growthStageData);

        // Update Sprites
        animationMonitor.UpdateSprites(characterConfig);

        // Update Speed
        characterMovement.SetSpeedMultiplier(characterConfig.speedMultiplier);

        // Update Scale
        characterTransform.localScale = Vector3.one * characterConfig.scale;

        // Update Camera Zoom
    }
}


