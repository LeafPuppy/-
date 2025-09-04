using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [Header("상태별 시작 노드")]
    [SerializeField] DialogueNode firstMeet;        //첫 만남 
    [SerializeField] DialogueNode preFirstEntry;    //첫 대화 후 ~ 첫 입장 전
    [SerializeField] DialogueNode postFirst_Clear;  //첫 던전 후 클리어(1회성)
    [SerializeField] DialogueNode postFirst_Death;  //첫 던전 후 사망(1회성)
    [SerializeField] DialogueNode preReEntry;       //두 번째 던전 진입

    public void Interact(Player player)
    {
        if (!DialogueUI.Instance) return;

        if (!DialogueUI.Instance.IsOpen)
        {
            var start = ResolveStartNode();
            if (start == null) return;

            var gs = GameState.Instance;

            if (!gs.hasMetMerchant && start == firstMeet)
                gs.OnTalkedToMerchantFirstTime();

            if (gs.totalRunsCompleted > gs.lastHandledRunIndex)
            {
                gs.lastHandledRunIndex = gs.totalRunsCompleted;
            }

            DialogueUI.Instance.Show(start);
        }
        else
        {
            DialogueUI.Instance.OnInteractPressed();
        }
    }

    DialogueNode ResolveStartNode()
    {
        var gs = GameState.Instance;

        //완전 처음(상인을 아직 못 만남)
        if (!gs.hasMetMerchant) return firstMeet;

        //아직 던전에 한 번도 안 들어갔으면(= 첫 대화 후 ~ 첫 입장 전)
        if (!gs.everEnteredDungeon) return preFirstEntry;

        //방금 전 런에서 마을로 돌아왔고, 아직 귀환 후 첫 대화를 안 한 경우
        bool justReturnedFromRun = gs.totalRunsCompleted > gs.lastHandledRunIndex;
        if (justReturnedFromRun)
        {
            bool isFirstReturnEver = (gs.totalRunsCompleted == 1);

            if (isFirstReturnEver)
            {
                if (gs.lastRunOutcome == RunOutcome.Cleared && postFirst_Clear) return postFirst_Clear;
                if (gs.lastRunOutcome == RunOutcome.Died && postFirst_Death) return postFirst_Death;
            }

            return preReEntry;
        }

        //그 외(두 번째 이후 재입장 준비 대화)
        return preReEntry;
    }
}
