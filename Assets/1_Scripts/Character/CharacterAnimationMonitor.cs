using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class CharacterAnimationMonitor : MonoBehaviour
{
   // [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, NaughtyAttributes.ReadOnly] private SerializedDictionary<Directions, Sprite> sprites = new();

    private Sprite prevSprite;

    public void UpdateSprites(SerializedDictionary<Directions, Sprite> sprites )
    {
        this.sprites = sprites;
    }

    private void UpdateAnimation(Vector2 input)
    {
        // determine sprite to use based on input direction

        if(input == Vector2.zero)
        {
            if(prevSprite == null)
                SetSprite(sprites[Directions.Down]);
        }

        if(input.x > 0)
        {
            SetSprite(sprites[Directions.Right]);
        }

        if(input.x < 0)
        {
            SetSprite(sprites[Directions.Left]);
        }

        if(input.y > 0)
        {
            SetSprite(sprites[Directions.Up]);
        }

        if(input.y < 0)
        {
            SetSprite(sprites[Directions.Down]);
        }
    }

    private void Update()
    {
        // todo: use joystick values
        Vector2 input = Vector2.zero;

        UpdateAnimation(input);
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        prevSprite = spriteRenderer.sprite;
    }
}
