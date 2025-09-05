using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LoadBtn : MonoBehaviour
{
    private void Awake()
    {
        if(!File.Exists(Application.persistentDataPath + "/PlayerData.json") || !File.Exists(Application.persistentDataPath + "/Setting.json"))
        {
            this.gameObject.SetActive(false);
        }
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            DataManager.Instance.LoadData();
            //CharacterManager.Instance.Player.condition.jem = DataManager.Instance.data.jem;
            //CharacterManager.Instance.Player.condition.dungeonClearCount = DataManager.Instance.data.dungeonClearCount;
        }
        if (File.Exists(Application.persistentDataPath + "/Setting.json"))
        {
            DataManager.Instance.LoadSetting();
            AudioManager.Instance.BGMSource.volume = DataManager.Instance.setting.bgmVolume;
            AudioManager.Instance.SFXSource.volume = DataManager.Instance.setting.sfxVolume;
            AudioManager.Instance.BGMSource.mute = DataManager.Instance.setting.bgmMute;
            AudioManager.Instance.SFXSource.mute = DataManager.Instance.setting.sfxMute;
        }

        SceneLoadManager.Instance.ChangeScene("VillageScene", () =>
        {
            AudioManager.Instance.BGMSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>("VillageBGM", eAssetType.Audio, eCategoryType.BGM);
            AudioManager.Instance.BGMSource.Play();
        });
    }
}
