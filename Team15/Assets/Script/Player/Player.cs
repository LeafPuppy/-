using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerAnimationController animator;
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
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        condition.state = PlayerState.Damage;
    }
}
