using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PopupType
{
    AddScore,
    MinusScore
}

public class PopupNumber : MonoBehaviour
{
    [SerializeField] private string prefix = "";
    [SerializeField] private TMP_Text text;
    [SerializeField] private float speed = 1;
    [SerializeField] private float lifetime = 1f;

    [Header("Colors")]
    [SerializeField] private Color color_normal = Color.white;

    private float elapsedTime = 0f;

    private Vector3 startPos;
    private Vector3 endPos;

    private float LifeTime => lifetime;


    public static PopupNumber Create(PopupType popupType, Vector3 position, int score, Transform parent)
    {
        GameObject damagePopupGO = Instantiate(GetObjectToInstantiate(popupType), position, Quaternion.identity, parent);
        PopupNumber damagePopup = damagePopupGO.GetComponent<PopupNumber>();
        damagePopup.Setup(score);
        return damagePopup;
    }

    private static GameObject GetObjectToInstantiate(PopupType popupType)
    {
        switch (popupType)
        {
            case PopupType.AddScore:
                return GameAssets.i.pfScorePopup_Add;
            case PopupType.MinusScore:
                return GameAssets.i.pfScorePopup_Minus;

            default: return null;
        }
    }

    private void Update()
    {
        UpdatePositionAndScale();

        UpdateLifeTime();
    }

    private void Setup(int score)
    {
        // damage amount
        text.text = $"{prefix}{score}";
        text.color = color_normal;

        // setup positions
        startPos = transform.position;
        endPos = (Vector2)transform.position + GetRandOffset();

        Vector2 GetRandOffset()
        {
            float radius = 50f;

            float randX = Random.Range(-.25f, .25f);
            float randY = Random.Range(-1f, 0f);

            Vector2 offset = new Vector2(randX, randY) * radius;
            return offset;
        }
    }


    private void UpdatePositionAndScale()
    {
        float t = elapsedTime / LifeTime;

        // move
        Vector3 movement = Vector3.Slerp(startPos, endPos, t);
        transform.position = movement;

        // fade
        float alpha = Mathf.Lerp(1, 0, t);
        text.alpha = alpha;

        // scale
        float halfLifeTime = LifeTime * .5f;
        if (elapsedTime < halfLifeTime)
        {
            // first half, scale up
            float scaleIncreaseAmount = 1f;
            transform.localScale += Vector3.one * scaleIncreaseAmount * Time.deltaTime;
        }
        else
        {
            // second half, scale down
            float scaleDecreaseAmount = 1f;
            transform.localScale -= Vector3.one * scaleDecreaseAmount * Time.deltaTime;
        }
    }

    private void UpdateLifeTime()
    {
        if (elapsedTime > LifeTime)
        {
            Destroy(gameObject);
        }

        elapsedTime += Time.deltaTime;
    }
}
