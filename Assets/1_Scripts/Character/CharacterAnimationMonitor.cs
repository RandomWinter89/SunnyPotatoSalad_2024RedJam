using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class CharacterAnimationMonitor : MonoBehaviour
{
   // [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, NaughtyAttributes.ReadOnly] private SerializedDictionary<Directions, SpriteSeries> sprites = new();

    private Sprite prevSprite;

    private SpriteSeries currentSpriteSeries = null;

    public void UpdateSprites(SerializedDictionary<Directions, SpriteSeries> sprites )
    {
        this.sprites = sprites;

    }


    private void Update()
    {
        // todo: use joystick values
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        UpdateAnimation(input);
    }


    private void UpdateAnimation(Vector2 input)
    {
        // determine sprite to use based on input direction

        if (input == Vector2.zero)
        {
            if (prevSprite == null)
                SetSpriteSeries(sprites[Directions.Down]);
        }

        if (input.x > 0)
        {
            SetSpriteSeries(sprites[Directions.Right]);
        }

        if (input.x < 0)
        {
            SetSpriteSeries(sprites[Directions.Left]);
        }

        if (input.y > 0)
        {
            SetSpriteSeries(sprites[Directions.Up]);
        }

        if (input.y < 0)
        {
            SetSpriteSeries(sprites[Directions.Down]);
        }

        currentSpriteSeries.Update();
        SetSprite(currentSpriteSeries.CurrentSprite);
    }


    private void SetSpriteSeries(SpriteSeries spriteSeries)
    {
        this.currentSpriteSeries = spriteSeries;
        spriteRenderer.flipX = currentSpriteSeries.flip;
        currentSpriteSeries.Enter();
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        prevSprite = spriteRenderer.sprite;
    }
}
