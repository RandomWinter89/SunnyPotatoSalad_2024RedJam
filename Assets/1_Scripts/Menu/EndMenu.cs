using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    [SerializeField] Button _restart;

    private void Awake()
    {
        _restart.onClick.AddListener(ReturnMainMenu);
    }

    private void ReturnMainMenu()
    {
        SceneLoader.instance.Load(Scene.Menu);
    }
}
