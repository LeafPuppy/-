using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 따라갈 타겟(플레이어)
    public string mapTag = "sampletag"; // 맵 경계 오브젝트 태그

    private Camera cam;
    private float halfHeight;
    private float halfWidth;
    private Vector2 minPosition;
    private Vector2 maxPosition;

    void Awake()
    {
        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        CalculateMapBounds();
    }

    void LateUpdate()
    {
        if (target == null) return;

        float clampX = Mathf.Clamp(target.position.x, minPosition.x + halfWidth, maxPosition.x - halfWidth);
        float centerY = target.position.y + 3f;

        // 카메라 z축을 -10으로 고정
        transform.position = new Vector3(clampX, centerY, -10f);
    }

    void CalculateMapBounds()
    {
        GameObject[] mapObjects = GameObject.FindGameObjectsWithTag(mapTag);
        if (mapObjects.Length == 0)
        {
            Debug.LogWarning($"태그 '{mapTag}'를 가진 오브젝트가 없습니다.");
            minPosition = Vector2.zero;
            maxPosition = Vector2.zero;
            return;
        }

        float minX = mapObjects[0].transform.position.x;
        float maxX = minX;

        foreach (GameObject obj in mapObjects)
        {
            float x = obj.transform.position.x;
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
        }

        minPosition = new Vector2(minX, 0f);
        maxPosition = new Vector2(maxX, 0f);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 경계 박스 그리기
        Vector3 bottomLeft = new Vector3(minPosition.x, minPosition.y, 0f);
        Vector3 topLeft = new Vector3(minPosition.x, maxPosition.y, 0f);
        Vector3 topRight = new Vector3(maxPosition.x, maxPosition.y, 0f);
        Vector3 bottomRight = new Vector3(maxPosition.x, minPosition.y, 0f);

        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }

}
