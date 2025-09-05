using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBtn : MonoBehaviour
{
    public void OnClickSave()
    {
        //DataManager.Instance.data.dungeonClearCount = CharacterManager.Instance.Player.condition.dungeonClearCount;
        //DataManager.Instance.data.jem = CharacterManager.Instance.Player.condition.jem;
        DataManager.Instance.setting.bgmVolume = AudioManager.Instance.BGMSource.volume;
        DataManager.Instance.setting.sfxVolume = AudioManager.Instance.SFXSource.volume;
        DataManager.Instance.setting.bgmMute = AudioManager.Instance.BGMSource.mute;
        DataManager.Instance.setting.sfxMute = AudioManager.Instance.SFXSource.mute;

        DataManager.Instance.SaveData();
        DataManager.Instance.SaveSetting();
    }
}
