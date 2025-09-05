using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : UIBase
{
    public void OnClickBtn(string str)
    {
        switch(str)
        {
            case "PauseUI":
                UIManager.Instance.Show<PauseUI>();
                break;
            case "SetUI":
                UIManager.Instance.Show<SetUI>();
                break;
            case "Exit":
                ExitBtn();
                break;
        }

        Hide();
    }

    public void ExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
