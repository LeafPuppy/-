using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAssetType
{
    Sprite,
    UI,
}

public enum eCategoryType
{
    None,
    Weapon,
}

public class ResourceManager : Singleton<ResourceManager>
{
    protected override bool isDestroy => false;
    
    public T LoadAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None) where T : Object
    {
        T handle = default;

        var typeStr = $"{assetType}/{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}/{key}";

        var obj = Resources.Load(typeStr, typeof(T));

        if (obj == null)
            return default;

        handle = (T)obj;

        return handle;
    }
}
