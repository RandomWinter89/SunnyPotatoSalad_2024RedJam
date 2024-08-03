using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class CharacterAnimationMonitor : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, NaughtyAttributes.ReadOnly] private SerializedDictionary<Directions, AnimationClipData> animationClipDatas = new();
    [SerializeField, NaughtyAttributes.ReadOnly] private SerializedDictionary<string, AnimationClipData> specialAnimationClipDatas = new();

    private AnimationClipData _prevAnimationClipData = null;

    public void UpdateSprites(CharacterConfig characterConfig)
    {
        this.animationClipDatas = characterConfig.animationClipDatas;
        this.specialAnimationClipDatas = characterConfig.specialAnimationClipDatas;
    }

    private void Start()
    {
        SetAnimationClip(animationClipDatas[Directions.Right]);
    }


    public void UpdateAnimation(Vector2 velocity)
    {

        if (velocity == Vector2.zero)
        {
            anim.speed = 0f;
            anim.PlayLastFrame();
        }
        else
        {
            anim.speed = 1f;

            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                // Horizontal movement has priority
                if (velocity.x > 0)
                {
                    SetAnimationClip(animationClipDatas[Directions.Right]);
                }
                else
                {
                    SetAnimationClip(animationClipDatas[Directions.Left]);
                }
            }
            else
            {
                // Vertical movement has priority
                if (velocity.y > 0)
                {
                    SetAnimationClip(animationClipDatas[Directions.Up]);
                }
                else
                {
                    SetAnimationClip(animationClipDatas[Directions.Down]);
                }
            }
        }
    }


    private void SetAnimationClip(AnimationClipData animationClipData)
    {
        if (_prevAnimationClipData != null)
        {
            if (animationClipData.priority < _prevAnimationClipData.priority) return;
        }

        anim.Play(animationClipData.clipName);
        spriteRenderer.flipX = animationClipData.flip;

        _prevAnimationClipData = animationClipData;
    }

    public void SetSpecialAnimationClip(string clipName)
    {
        AnimationClipData animationClipData = specialAnimationClipDatas[clipName];
        SetAnimationClip(animationClipData);
    }
}
