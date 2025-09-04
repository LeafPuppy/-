using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ���� Ÿ��(�÷��̾�)
    public string mapTag = "sampletag"; // �� ��� ������Ʈ �±�

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

        // ī�޶� z���� -10���� ����
        transform.position = new Vector3(clampX, centerY, -10f);
    }

    void CalculateMapBounds()
    {
        GameObject[] mapObjects = GameObject.FindGameObjectsWithTag(mapTag);
        if (mapObjects.Length == 0)
        {
            Debug.LogWarning($"�±� '{mapTag}'�� ���� ������Ʈ�� �����ϴ�.");
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
        // ��� �ڽ� �׸���
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
