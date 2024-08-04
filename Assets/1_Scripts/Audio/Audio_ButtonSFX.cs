using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class Audio_ButtonSFX : MonoBehaviour
{
    [SerializeField, Dropdown("options")] string audioName;

    private static List<string> options
    {
        get {
            return new List<string>()
            {
                "SelectButton", "SelectButton2", "ShortClick", "EnterGameplay"
            };
        }
    }

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button)
            button.onClick.AddListener(Play);
    }

    public void Play()
    {
        AudioManager.instance.OnSimpleAction_SFXAudio(audioName);
    }
}
