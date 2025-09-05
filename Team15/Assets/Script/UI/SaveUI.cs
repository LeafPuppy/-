using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : UIBase
{
    public void OnClickYes()
    {
        AudioManager.Instance.PlaySFX("BtnSFX");
        DataManager.Instance.ResetData();
        SceneLoadManager.Instance.ChangeScene("VillageScene");
    }

    public void OnClickExit()
    {
        AudioManager.Instance.PlaySFX("BtnSFX");
        Hide();
    }
}
