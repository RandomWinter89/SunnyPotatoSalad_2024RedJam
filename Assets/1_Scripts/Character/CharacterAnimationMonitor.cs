using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Security.Cryptography.X509Certificates;


public class CharacterAnimationMonitor : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, NaughtyAttributes.ReadOnly] private SerializedDictionary<Directions, AnimationClipData> animationClipDatas = new();

    private AnimationClipData _prevAnimationClipData = null;

    public void UpdateSprites(SerializedDictionary<Directions, AnimationClipData> animationClipDatas)
    {
        this.animationClipDatas = animationClipDatas;

    }


    private void Update()
    {
        // todo: use joystick values
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        UpdateAnimation(input);
    }

    private void UpdateAnimation(Vector2 input)
    {
        // Determine sprite to use based on input direction

        if (input == Vector2.zero)
        {
            if(_prevAnimationClipData == null)
            {
                SetAnimationClip(animationClipDatas[Directions.Right]);
            }
            anim.speed = 0f;
            anim.PlayLastFrame();
        }
        else
        {
            anim.speed = 1f;

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                // Horizontal movement has priority
                if (input.x > 0)
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
                if (input.y > 0)
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
        anim.Play(animationClipData.clipName);
        spriteRenderer.flipX = animationClipData.flip;

        _prevAnimationClipData = animationClipData;

    }
}
