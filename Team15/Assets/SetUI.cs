using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SetUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject bgmMute;
    public GameObject sfxMute;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/Setting.json"))
        {
            DataManager.Instance.LoadSetting();
            AudioManager.Instance.BGMSource.volume = DataManager.Instance.setting.bgmVolume;
            AudioManager.Instance.SFXSource.volume = DataManager.Instance.setting.sfxVolume;
            AudioManager.Instance.BGMSource.mute = DataManager.Instance.setting.bgmMute;
            AudioManager.Instance.SFXSource.mute = DataManager.Instance.setting.sfxMute;
            bgmSlider.value = DataManager.Instance.setting.bgmVolume;
            sfxSlider.value = DataManager.Instance.setting.sfxVolume;
            bgmMute.SetActive(DataManager.Instance.setting.bgmMute);
            sfxMute.SetActive(DataManager.Instance.setting.sfxMute);
        }
        else
        {
            AudioManager.Instance.BGMSource.volume = 1;
            AudioManager.Instance.SFXSource.volume = 1;
            AudioManager.Instance.BGMSource.mute = false;
            AudioManager.Instance.SFXSource.mute = false;
            bgmSlider.value = 1;
            sfxSlider.value = 1;
        }

        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(File.Exists(Application.persistentDataPath + "/Setting.json"))
        {
            DataManager.Instance.LoadSetting();
            AudioManager.Instance.BGMSource.volume = DataManager.Instance.setting.bgmVolume;
            AudioManager.Instance.SFXSource.volume = DataManager.Instance.setting.sfxVolume;
            AudioManager.Instance.BGMSource.mute = DataManager.Instance.setting.bgmMute;
            AudioManager.Instance.SFXSource.mute = DataManager.Instance.setting.sfxMute;
            bgmSlider.value = DataManager.Instance.setting.bgmVolume;
            sfxSlider.value = DataManager.Instance.setting.sfxVolume;
            bgmMute.SetActive(DataManager.Instance.setting.bgmMute);
            sfxMute.SetActive(DataManager.Instance.setting.sfxMute);
        }
        else
        {
            AudioManager.Instance.BGMSource.volume = 1;
            AudioManager.Instance.SFXSource.volume = 1;
            AudioManager.Instance.BGMSource.mute = false;
            AudioManager.Instance.SFXSource.mute = false;
            bgmSlider.value = 1;
            sfxSlider.value = 1;
        }
    }

    private void OnDisable()
    {
        DataManager.Instance.setting.bgmVolume = AudioManager.Instance.BGMSource.volume;
        DataManager.Instance.setting.sfxVolume = AudioManager.Instance.SFXSource.volume;
        DataManager.Instance.setting.bgmMute = AudioManager.Instance.BGMSource.mute;
        DataManager.Instance.setting.sfxMute = AudioManager.Instance.SFXSource.mute;
        DataManager.Instance.SaveSetting();
    }
}
