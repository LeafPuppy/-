using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    protected override bool isDestroy => false;

    public string NowSceneName = "";

    protected override void Awake()
    {
        base.Awake();
        
        NowSceneName = SceneManager.GetActiveScene().name;
    }

    public async void ChangeScene(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);

        while(!op.isDone)
        {
            Debug.Log("로딩중");
            await Task.Yield();
        }

        Debug.Log("로딩 완료");
    }
}
