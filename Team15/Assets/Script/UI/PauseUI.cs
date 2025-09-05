using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : UIBase
{
    public void OnClickResumeBtn()
    {
        Hide();
        Time.timeScale = 1f;
    }

    public void OnClickSetBtn()
    {
        Hide();
        UIManager.Instance.Show<SetUI>();
    }

    public void OnClickDungeonExitBtn()
    {
        Hide();
        SceneLoadManager.Instance.ChangeScene("VillageScene");
    }
}
