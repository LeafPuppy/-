using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : UIBase
{
    public void OnClickResumeBtn()
    {
        AudioManager.Instance.PlaySFX("BtnSFX");
        Hide();
        Time.timeScale = 1f;
    }

    public void OnClickSetBtn()
    {
        AudioManager.Instance.PlaySFX("BtnSFX");
        Hide();
        UIManager.Instance.Show<SetUI>();
    }

    public void OnClickDungeonExitBtn()
    {
        AudioManager.Instance.PlaySFX("BtnSFX");
        Hide();
        SceneLoadManager.Instance.ChangeScene("VillageScene");
    }
}
