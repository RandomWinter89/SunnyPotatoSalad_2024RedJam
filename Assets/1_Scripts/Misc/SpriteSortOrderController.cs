using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrderController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer dependentSpriteRenderer;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RunUpdate()
    {
        if(dependentSpriteRenderer != null)
        {
            spriteRenderer.sortingOrder = ((int)dependentSpriteRenderer.transform.position.z - 1) * -1;
            return;
        }
        else
        {
            spriteRenderer.sortingOrder = ((int)transform.position.z) * -1;
        }
    
    }
}
