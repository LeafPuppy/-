using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SetUI : UIBase
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject bgmMute;
    public GameObject sfxMute;

    public Button bgmOff;
    public Button bgmOn;
    public Button sfxOff;
    public Button sfxOn;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        bgmSource = AudioManager.Instance.BGMSource;
        sfxSource = AudioManager.Instance.SFXSource;

        bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        bgmOff.onClick.AddListener(BGMOff);
        sfxOff.onClick.AddListener(SFXOff);
        bgmOn.onClick.AddListener(BGMOn);
        sfxOn.onClick.AddListener(SFXOn);

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

    private void OnDestroy()
    {
        DataManager.Instance.setting.bgmVolume = AudioManager.Instance.BGMSource.volume;
        DataManager.Instance.setting.sfxVolume = AudioManager.Instance.SFXSource.volume;
        DataManager.Instance.setting.bgmMute = AudioManager.Instance.BGMSource.mute;
        DataManager.Instance.setting.sfxMute = AudioManager.Instance.SFXSource.mute;
        DataManager.Instance.SaveSetting();
    }

    private void ChangeBGMVolume(float value)
    {
        bgmSource.volume = value;
    }

    private void ChangeSFXVolume(float value)
    {
        sfxSource.volume = value;
    }
    private void BGMOff()
    {
        bgmSource.mute = true;
    }
    private void SFXOff()
    {
        sfxSource.mute = true;
    }
    private void BGMOn()
    {
        bgmSource.mute = false;
    }
    private void SFXOn()
    {
        sfxSource.mute = false;
    }

    public void OnClickExit()
    {
        Hide();
        UIManager.Instance.Show<TitleUI>();
    }
}
