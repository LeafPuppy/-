using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public Transform target;
    public float offsetY = 1.5f; // 월드 기준 위쪽 거리

    void LateUpdate()
    {
        if (!target) return;

        // 오브젝트 위치 + 월드 up 방향으로 오프셋
        transform.position = target.position + Vector3.up * offsetY;

        // 회전은 항상 고정
        transform.rotation = Quaternion.identity;
    }
}
