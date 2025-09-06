using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ResultUI : UIBase
{
    public TextMeshProUGUI playTime;
    public TextMeshProUGUI killCount;
    public TextMeshProUGUI getCount;
    public TextMeshProUGUI dropCount;
    public TextMeshProUGUI tileCount;
    public TextMeshProUGUI deadCount;

    private void Awake()
    {
        if(File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            DataManager.Instance.LoadData();
            //playTime = DataManager.Instance.data.playTime;
            //killCount = DataManager.Instance.data.killCount;
            //getCount = DataManager.Instance.data.getCount;
            //dropCount = DataManager.Instance.data.dropCount;
            //tileCount = DataManager.Instance.data.tileCount;
            //deadCount = DataManager.Instance.data.deadCount;
        }
    }

    public void OnClickVillageBtn()
    {
        Hide();
        SceneLoadManager.Instance.ChangeScene("VillageScene");
    }

    public void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
