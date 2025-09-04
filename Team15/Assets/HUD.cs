using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image HPBar;
    public Image weapon;
    public GameObject dashCool;

    public TextMeshProUGUI jemCount;

    private void Update()
    {
        UpdateHp();
    }

    public void UpdateHp()
    {
        HPBar.fillAmount = CharacterManager.Instance.Player.currentHealth / CharacterManager.Instance.Player.maxHealth;
    }

    public void UpdateWeapon(string Name)
    {
        //스프라이트 이름이랑 무기 이름 같에 수정해서 받기
        weapon.sprite = ResourceManager.Instance.LoadAsset<Sprite>(name, eAssetType.Sprite, eCategoryType.Weapon);
    }

    public void UpdateJem(int amount)
    {
        //플레이어 컨디션에 젬 카운트 추가하고 더해주기
        jemCount.text = amount.ToString();
    }

    public void UpdateDash()
    {
        //플레이어 컨트롤러 isDashing public으로 풀기
        //dashCool.SetActive(CharacterManager.Instance.Player.controller.isDashing);
    }
}
