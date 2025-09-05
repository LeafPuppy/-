using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VillageScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();

        if (File.Exists(Application.persistentDataPath + "/Setting.json"))
        {
            DataManager.Instance.LoadSetting();
            AudioManager.Instance.BGMSource.volume = DataManager.Instance.setting.bgmVolume;
            AudioManager.Instance.SFXSource.volume = DataManager.Instance.setting.sfxVolume;
            AudioManager.Instance.BGMSource.mute = DataManager.Instance.setting.bgmMute;
            AudioManager.Instance.SFXSource.mute = DataManager.Instance.setting.sfxMute;
        }
        else
        {
            AudioManager.Instance.BGMSource.volume = 1;
            AudioManager.Instance.SFXSource.volume = 1;
            AudioManager.Instance.BGMSource.mute = false;
            AudioManager.Instance.SFXSource.mute = false;
            DataManager.Instance.SaveSetting();
        }
    }

    private void Start()
    {
        UIManager.Instance.Show<HUD>();
    }
}
