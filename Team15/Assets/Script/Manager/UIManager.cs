using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public static int ScreenWidth = 1920;
    public static int ScreenHeight = 1080;

    protected override bool isDestroy => false;

    private Dictionary<string, UIBase> ui_List = new Dictionary<string, UIBase>();

    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;

        ui_List.TryGetValue(uiName, out UIBase ui);

        if(ui == null)
        {
            ui = Load<T>(uiName);
            ui_List.Add(uiName, ui);
        }

        ui.gameObject.SetActive(true);

        return (T) ui;
    }

    public T Load<T>(string uiName) where T : UIBase
    {
        var newCanvasObject = new GameObject(uiName + "Canvas");

        var canvas = newCanvasObject.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);

        newCanvasObject.gameObject.AddComponent<GraphicRaycaster>();

        newCanvasObject.AddComponent<CanvasRenderer>();

        var prefab = ResourceManager.Instance.LoadAsset<GameObject>(uiName, eAssetType.UI);
        var obj = Instantiate(prefab, newCanvasObject.transform);
        obj.name = obj.name.Replace("(Clone)", "");

        var result = obj.GetComponent<T>();
        result.canvas = canvas;
        result.canvas.sortingOrder = ui_List.Count;

        return result;
    }

    public T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        ui_List.TryGetValue(uiName, out UIBase ui);

        if (ui == null)
        {
            Debug.LogError($"{uiName} don't exist");
            return default;
        }

        return (T)ui;
    }


    public void Hide<T>()
    {
        string uiName = typeof (T).Name;

        Hide(uiName);
    }

    public void Hide(string uiName)
    {
        ui_List.TryGetValue(uiName, out UIBase ui);

        if (ui == null)
            return;

        DestroyImmediate(ui.canvas.gameObject);
        ui_List.Remove(uiName);
    }
}
