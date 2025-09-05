using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    public bool isManager = false;

    protected override bool isDestroy => false;

    public string NowSceneName = "";

    protected override void Awake()
    {
        base.Awake();

        NowSceneName = SceneManager.GetActiveScene().name;
    }

    public async void ChangeScene(string sceneName, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var op = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (!op.isDone)
        {
            Debug.Log("로딩중");
            await Task.Yield();
        }
        Debug.Log("로딩 완료");

        if(loadSceneMode == LoadSceneMode.Single)
        {
            NowSceneName = sceneName;
        }

        callback?.Invoke();
    }

    public async void UnLoadScene(string sceneName, Action callback = null)
    {
        var op = SceneManager.UnloadSceneAsync(sceneName);

        while(!op.isDone)
        {
            Debug.Log("씬 언로드 중...");
            await Task.Yield();
        }

        callback?.Invoke();
    }
}
