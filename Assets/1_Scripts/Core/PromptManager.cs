using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct PromptData
{
    public string title;
    public string description;
    public string confirmText;
    public Action action;
    public bool enablePanel; // on action invoked, set panel gameobject status
}

public class PromptManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descText;
    [SerializeField] TMP_Text confirmBtnText;
    [SerializeField] Button confirmBtn;

    private static event Action<PromptData> OnPromptEvent;

    private void OnEnable()
    {
        OnPromptEvent += OnPrompt;
    }

    private void OnDisable()
    {
        OnPromptEvent -= OnPrompt;
    }

    public static void Prompt(string title, string text, Action action = null)
    {
        OnPromptEvent?.Invoke(new PromptData { title = title, description = text, confirmText = "OK", action = action});
    }

    public static void Prompt(PromptData prompt)
    {
        OnPromptEvent?.Invoke(prompt);
    }

    private void OnPrompt(PromptData data)
    {
        titleText.SetText(data.title);
        descText.SetText(data.description);
        confirmBtnText.SetText(data.confirmText);
        confirmBtn.gameObject.SetActive(data.action != null);
        confirmBtn.onClick.RemoveAllListeners();

        if (data.action != null)
        {
            confirmBtn.onClick.AddListener(() =>
            {
                data.action?.Invoke();
                panel.SetActive(data.enablePanel);
            });
        }

        panel.SetActive(true);
    }
}
