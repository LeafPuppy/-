using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTestSCene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();

        var hud = UIManager.Instance.Show<HUD>();
    }
}
