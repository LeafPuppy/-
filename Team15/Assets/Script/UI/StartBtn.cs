using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    public void OnClickBtn()
    {
        SceneLoadManager.Instance.ChangeScene("VillageScene");
        AudioManager.Instance.BGMSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>("VillageBGM", eAssetType.Audio, eCategoryType.BGM);
        AudioManager.Instance.BGMSource.Play();
    }
}
