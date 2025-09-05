using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UIBase
{
    public Image HPBar;
    public Image weapon;
    public GameObject get;
    public GameObject dashCool;

    public TextMeshProUGUI jemCount;

    private void Update()
    {
        UpdateHp();
        UpdateWeapon();
        UpdateDash();
    }

    public void UpdateHp()
    {
        HPBar.fillAmount = CharacterManager.Instance.Player.currentHealth / CharacterManager.Instance.Player.maxHealth;
    }

    public void UpdateWeapon()
    {
        if (CharacterManager.Instance.Player.controller.weaponHolder.gameObject.transform.childCount != 0)
        {
            weapon.enabled = true;
            get.SetActive(false);
            var name = CharacterManager.Instance.Player.controller.weaponHolder.transform.GetChild(0).name;
            name = name.Replace("(Clone)", "");
            weapon.sprite = ResourceManager.Instance.LoadAsset<Sprite>(name, eAssetType.Sprite, eCategoryType.Weapon);
        }
        else
        {
            get.SetActive(true);
            weapon.enabled = false;
        }
    }

    public void UpdateJem(int amount)
    {
        //플레이어 컨디션에 젬 카운트 추가하고 더해주기
        jemCount.text = amount.ToString();
    }

    public void UpdateDash()
    {
        dashCool.SetActive(CharacterManager.Instance.Player.controller.isDashing);
    }
}
