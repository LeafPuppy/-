using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public Transform target;
    public float offsetY = 1.5f; // ���� ���� ���� �Ÿ�

    void LateUpdate()
    {
        if (!target) return;

        // ������Ʈ ��ġ + ���� up �������� ������
        transform.position = target.position + Vector3.up * offsetY;

        // ȸ���� �׻� ����
        transform.rotation = Quaternion.identity;
    }
}
