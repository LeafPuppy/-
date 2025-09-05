using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    public GameObject saveUI;

    public void OnClickBtn()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            DataManager.Instance.data = new DataManager.PlayerData();
            DataManager.Instance.data.dungeonClearCount = 0;
            DataManager.Instance.data.jem = 0;
            DataManager.Instance.SaveData();
        }
        else
        {
            saveUI.SetActive(true);
            return;
        }

        //if (!File.Exists(Application.persistentDataPath + "/Setting.json"))
        //{
        //    DataManager.Instance.setting = new DataManager.Setting();
        //    DataManager.Instance.setting.sfxVolume = 1f;
        //    DataManager.Instance.setting.bgmVolume = 1f;
        //    DataManager.Instance.setting.sfxMute = false;
        //    DataManager.Instance.setting.bgmMute = false;
        //    DataManager.Instance.SaveSetting();
        //}
        //else
        //{
        //    saveUI.SetActive(true);
        //    return;
        //}

        SceneLoadManager.Instance.ChangeScene("VillageScene");
        AudioManager.Instance.BGMSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>("VillageBGM", eAssetType.Audio, eCategoryType.BGM);
        AudioManager.Instance.BGMSource.Play();
    }
}
