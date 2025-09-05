using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : UIBase
{
    public void OnClickBtn(string str)
    {
        AudioManager.Instance.PlaySFX("BtnSFX");
        switch (str)
        {
            case "PauseUI":
                UIManager.Instance.Show<PauseUI>();
                break;
            case "SetUI":
                UIManager.Instance.Show<SetUI>();
                break;
            case "HUD":
                UIManager.Instance.Show<HUD>();
                break;
        }

        Hide();
    }
}
