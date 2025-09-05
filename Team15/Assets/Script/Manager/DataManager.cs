using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    protected override bool isDestroy => false;

    public PlayerData data;
    public Setting setting;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SaveData()
    {
        var saveData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", saveData);
        Debug.Log(Application.persistentDataPath + "/PlayerData.json");
    }

    public void SaveSetting()
    {
        var saveSetting = JsonUtility.ToJson(setting);
        File.WriteAllText(Application.persistentDataPath + "/Setting.json", saveSetting);
        Debug.Log(Application.persistentDataPath + "/Setting.json");
    }


    public void LoadData()
    {
        var loadData = File.ReadAllText(Application.persistentDataPath + "/PlayerData.json");
        data = JsonUtility.FromJson<PlayerData>(loadData);
    }

    public void LoadSetting()
    {
        var loadSetting = File.ReadAllText(Application.persistentDataPath + "/Setting.json");
        setting = JsonUtility.FromJson<Setting>(loadSetting);
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.json") && File.Exists(Application.persistentDataPath + "/Setting.json"))
        {
            LoadData();
            LoadSetting();
            data = new PlayerData();
            data.dungeonClearCount = 0;
            data.jem = 0;
            SaveData();
            setting = new Setting();
            setting.sfxVolume = 1f;
            setting.bgmVolume = 1f;
            setting.sfxMute = false;
            setting.bgmMute = false;
            SaveSetting();
        }
        else
            return;

    }

    [System.Serializable]
    public class PlayerData
    {
        public int dungeonClearCount;
        public int jem;
    }

    [System.Serializable]
    public class Setting
    {
        public float bgmVolume;
        public float sfxVolume;
        public bool bgmMute;
        public bool sfxMute;
    }
}
