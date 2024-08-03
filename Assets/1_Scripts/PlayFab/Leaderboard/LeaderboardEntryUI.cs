using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;

public class LeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Image positionImg;
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text profileNameText;
    [SerializeField] private TMP_Text valueText;

    public void Init(PlayerLeaderboardEntry entry)
    {
        int position = entry.Position + 1;

        if (IsUser(entry))
            backgroundImg.color = GetUserColor();

        positionImg.color = GetPositionColor(position);
        positionText.SetText(position.ToString());
        profileNameText.SetText(!string.IsNullOrEmpty(entry.DisplayName) ? entry.DisplayName : "User");
        valueText.SetText(entry.StatValue.ToString());

        var rect = transform as RectTransform;
        var parent = transform.parent as RectTransform;

        rect.sizeDelta = new Vector2(parent.rect.width, rect.sizeDelta.y);
    }

    private bool IsUser(PlayerLeaderboardEntry entry)
    {
        return entry.PlayFabId == DataManager.main.playFabID;
    }

    private Color GetUserColor()
    {
        ColorUtility.TryParseHtmlString("#AFE700", out Color color);
        return color;
    }

    private Color GetPositionColor(int position)
    {
        string htmlString;

        switch (position)
        {
            case 1:
                htmlString = "#938700";
                break;

            case 2:
                htmlString = "#737373";
                break;

            case 3:
                htmlString = "#7B4000";
                break;

            default:
                return positionImg.color;
        }

        if (ColorUtility.TryParseHtmlString(htmlString, out Color color))
            return color;
        else
            return positionImg.color;
    }
}
