using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        if (!SceneLoadManager.Instance.isManager)
        {
            SceneLoadManager.Instance.ChangeScene("Manager", () =>
            {
                SceneLoadManager.Instance.isManager = true;
                SceneLoadManager.Instance.UnLoadScene("Manager");
            }, LoadSceneMode.Additive);
        }
    }
}
