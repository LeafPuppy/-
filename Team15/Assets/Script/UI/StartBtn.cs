using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
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
            UIManager.Instance.Show<SaveUI>();
            return;
        }

        SceneLoadManager.Instance.ChangeScene("VillageScene", () =>
        {
            AudioManager.Instance.BGMSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>("VillageBGM", eAssetType.Audio, eCategoryType.BGM);
            AudioManager.Instance.BGMSource.Play();
        });
    }
}
