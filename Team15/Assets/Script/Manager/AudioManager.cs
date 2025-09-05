using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    protected override bool isDestroy => false;

    public AudioSource BGMSource;
    public AudioSource SFXSource;

    protected override void Awake()
    {
        base.Awake();
        BGMSource.playOnAwake = true;
        BGMSource.Play();
        BGMSource.loop = true;
    }

    public void PlaySFX(string clipName)
    {
        SFXSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>(clipName, eAssetType.Audio, eCategoryType.SFX);
        SFXSource.Play();
    }
}
