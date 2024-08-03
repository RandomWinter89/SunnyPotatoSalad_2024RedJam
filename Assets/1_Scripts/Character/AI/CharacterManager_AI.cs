using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterManager_AI : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;

    [Space]
    [SerializeField] private CharacterMovement_AI characterMovement;
    [SerializeField] private CharacterGrowth characterGrowth;
    [SerializeField] private CharacterAnimationMonitor animationMonitor;
    [SerializeField] private Collider collider;

    public CharacterConfig CharacterConfig { get; set; }

    private bool _isDead = false;

    public event Action OnDead;

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void Awake()
    {
        characterGrowth.OnGrowthStageUpdated += OnGrowthStageUpdated;
        OnGrowthStageUpdated(characterGrowth.FirstGrowthStageData);
    }

    private void Update()
    {
        if (_isDead) return;
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
    }

    public void Die()
    {
        _isDead = true;

        animationMonitor.SetSpecialAnimationClip("Squashed");

        // stop movement
        characterMovement.Stop();

        // stop physics
        collider.enabled = false;

        // inform spawner
        OnDead?.Invoke();

        StartCoroutine(DisableAfterDeathRoutine());
    }

    private IEnumerator DisableAfterDeathRoutine()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }


}
