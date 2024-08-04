using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static PlayerPrefKeys;

public class LoginManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isAutoLogin = true;

    [Header("References")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;

    public static Action OnLogin;

    private void Awake()
    {
        if (isAutoLogin && PlayerPrefs.HasKey(EMAIL) && PlayerPrefs.HasKey(PASSWORD))
        {
            string prefs_email = PlayerPrefs.GetString(EMAIL);
            string prefs_password = PlayerPrefs.GetString(PASSWORD);

            emailInput.SetTextWithoutNotify(prefs_email);
            passwordInput.SetTextWithoutNotify(prefs_password);

            Login(prefs_email, prefs_password);
        }

        loginButton.onClick.AddListener(() => Login(emailInput.text, passwordInput.text));
    }

    public void Login(string email, string password)
    {
        StartCoroutine(PlayFabUtils.Login(email, password, GoToMenu));
    }

    public void GoToMenu()
    {
        OnLogin?.Invoke();

        PlayerPrefs.SetString(EMAIL, emailInput.text);
        PlayerPrefs.SetString(PASSWORD, passwordInput.text);

        SceneLoader.instance.Load(Scene.Menu, DataManager.main.LoadPlayerDataRoutine());
    }

    public static void ForgetAllCredentials()
    {
        PlayerPrefs.DeleteKey(EMAIL);
        PlayerPrefs.DeleteKey(PASSWORD);
    }
}
