using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum Scene
{
    Login,
    Menu,
    Gameplay,
}

public class SceneLoader : MonoBehaviour
{
    public const string LoginScene = "S_Login";
    public const string MenuScene = "S_Menu";
    public const string Gameplay = "S_Gameplay";

    private static Dictionary<Scene, string> sceneMap = new()
    {
        { Scene.Login, LoginScene },
        { Scene.Menu, MenuScene },
        { Scene.Gameplay, Gameplay },
    };

    public event Action OnLoadStart;
    public event Action OnLoadEnd;

    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    public void Load(Scene scene, IEnumerator routine = null)
    {
        Loading.Load();
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            Time.timeScale = 1;
            OnLoadStart?.Invoke();

            if (routine != null)
                yield return routine;

            Load(sceneMap[scene]);
        }
    }

    private void Load(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine());

        IEnumerator LoadSceneRoutine()
        {
            AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!asyncLoadScene.isDone) yield return null;

            OnLoadEnd?.Invoke();

            yield return new WaitForSecondsRealtime(.25f);
            Loading.Stop();
        }
    }
}
