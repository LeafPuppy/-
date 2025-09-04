using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    protected override bool isDestroy => false;

    [SerializeField] private GameObject[] ui;

    public void ShowUI(int index)
    {
        ui[index].gameObject.SetActive(true);
    }

    public void HideUI(int index)
    {
        ui[index].gameObject.SetActive(false);
    }
}
