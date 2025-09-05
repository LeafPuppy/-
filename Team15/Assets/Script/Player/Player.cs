using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerAnimationController animator;
    public GameObject weaponHolder;
    //public TalkScript talk;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        //talk = GetComponent<TalkScript>();
    }

    private void Update()
    {
        animator.ChangeAnimation(condition.state);
        if(weaponHolder.transform.childCount > 0)
        {
            condition.isHoldWeapon = true;
        }
        else
        {
            condition.isHoldWeapon = false;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        AudioManager.Instance.PlaySFX("PlayerDamageSFX");
        condition.state = AnimationState.Damage;
    }
}
