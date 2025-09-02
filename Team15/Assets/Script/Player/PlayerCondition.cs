using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    Idle,
    Move,
    Jump,
    Damage,
    Die,
}

public class PlayerCondition : MonoBehaviour
{
    public PlayerState state = PlayerState.Idle;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
