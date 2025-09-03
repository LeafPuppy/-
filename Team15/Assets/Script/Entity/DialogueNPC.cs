using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [Header("���º� ���� ���")]
    [SerializeField] DialogueNode firstMeet;        //ù ���� 
    [SerializeField] DialogueNode preFirstEntry;    //ù ��ȭ �� ~ ù ���� ��
    [SerializeField] DialogueNode postFirst_Clear;  //ù ���� �� Ŭ����
    [SerializeField] DialogueNode postFirst_Death;  //ù ���� �� ���
    [SerializeField] DialogueNode preReEntry;       //�� ��° ���� ����

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

            if (gs.totalRunsCompleted > gs.lastHandledRunIndex && (start == postFirst_Clear || start == postFirst_Death))
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

        //���� ó��(������ ���� �� ����)
        if (!gs.hasMetMerchant) return firstMeet;

        //���� ������ �� ���� �� ������(= ù ��ȭ �� ~ ù ���� ��)
        if (!gs.everEnteredDungeon) return preFirstEntry;

        //��� �� ������ ������ ���ƿ԰�, ���� ��ȯ �� ù ��ȭ�� �� �� ���
        bool justReturnedFromRun = gs.totalRunsCompleted > gs.lastHandledRunIndex;
        if (justReturnedFromRun)
        {
            if (gs.lastRunOutcome == RunOutcome.Cleared && postFirst_Clear) return postFirst_Clear;
            if (gs.lastRunOutcome == RunOutcome.Died && postFirst_Death) return postFirst_Death;
        }

        //�� ��(�� ��° ���� ������ �غ� ��ȭ)
        return preReEntry;
    }
}
