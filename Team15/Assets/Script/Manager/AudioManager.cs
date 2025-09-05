using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    protected override bool isDestroy => false;

    public AudioSource BGMSource;
    public AudioSource SFXSource;

    protected override void Awake()
    {
        base.Awake();

        if (transform.childCount == 0)
            CreateAudioSource();

        BGMSource.playOnAwake = true;
        BGMSource.Play();
        BGMSource.loop = true;
    }

    public void PlaySFX(string clipName)
    {
        SFXSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>(clipName, eAssetType.Audio, eCategoryType.SFX);
        SFXSource.Play();
    }
    public void StopSFX()
    {
        SFXSource.Stop();
    }

    private void CreateAudioSource()
    {
        GameObject BGM = new GameObject("BGM");
        var bgm = Instantiate(BGM, this.transform);
        bgm.AddComponent<AudioSource>();

        GameObject SFX = new GameObject("SFX");
        var sfx = Instantiate(SFX, this.transform);
        sfx.AddComponent<AudioSource>();

        BGMSource = bgm.GetComponent<AudioSource>();
        SFXSource = sfx.GetComponent<AudioSource>();
    }
}
