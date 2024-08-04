using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneLoader_Instance : MonoBehaviour
{
    [SerializeField] Scene loadScene;

    public void LoadScene()
    {
        SceneLoader.instance.Load(loadScene);
    }

    public void LoadScene(string sceneName)
    {
        Enum.TryParse(typeof(Scene), sceneName, out object scene);
        SceneLoader.instance.Load((Scene)scene);
    }
}
