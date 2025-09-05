using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapSelectUI : MonoBehaviour
{
    public static MapSelectUI Instance { get; private set; }

    [Header("패널/라벨")]
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text floorText;

    [Header("노드들(씬의 버튼들)")]
    [SerializeField] List<MapNodeButton> nodes = new();

    [Header("화살표 연결(간선)  from -> to")]
    [SerializeField] List<Edge> edges = new();

    [Header("시작 노드 ID")]
    [SerializeField] string startNodeId = "Start";

    // 진행 상태
    string currentNodeId;
    HashSet<string> cleared = new();   // 지나간 노드(재진입 금지)
    HashSet<string> visited = new();   // 방문한 적 있는 노드 기록

    // 내부 맵
    Dictionary<string, MapNodeButton> nodeMap;
    Dictionary<string, List<string>> adj;

    void Awake()
    {
        Instance = this;
        if (panel) panel.SetActive(false);

        nodeMap = new Dictionary<string, MapNodeButton>();

        Debug.Log("[MapSelectUI] Awake 시작 - nodes.Count = " + nodes.Count);

        int i = 0;
        foreach (var n in nodes)
        {
            if (n == null)
            {
                Debug.LogError($"[MapSelectUI] nodes[{i}] 이 null입니다! 인스펙터 확인 필요");
                i++;
                continue;
            }

            if (string.IsNullOrEmpty(n.nodeId))
                Debug.LogError($"[MapSelectUI] {n.name} 의 nodeId 비어있음!");

            if (nodeMap.ContainsKey(n.nodeId))
                Debug.LogError($"[MapSelectUI] nodeId 중복됨: {n.nodeId}");

            nodeMap[n.nodeId] = n;
            Debug.Log($"[MapSelectUI] 등록된 노드[{i}]: {n.name}, nodeId={n.nodeId}, type={n.type}");
            i++;
        }

        adj = new Dictionary<string, List<string>>();
        foreach (var e in edges)
        {
            if (!adj.ContainsKey(e.from)) adj[e.from] = new List<string>();
            adj[e.from].Add(e.to);

            if (!nodeMap.ContainsKey(e.from))
                Debug.LogError("[MapSelectUI] Edge.from 존재 안함: " + e.from);
            if (!nodeMap.ContainsKey(e.to))
                Debug.LogError("[MapSelectUI] Edge.to 존재 안함: " + e.to);
        }

        currentNodeId = startNodeId;
        visited.Add(currentNodeId);

        Debug.Log("[MapSelectUI] Awake 완료. 시작 노드: " + currentNodeId);
    }

    public void Open(int floor = 1)
    {
        if (panel)
        {
            panel.SetActive(true);
            Debug.Log("[MapSelectUI] panel 활성화됨: " + panel.name + ", activeSelf=" + panel.activeSelf);
        }
        else
        {
            Debug.LogWarning("[MapSelectUI] panel 참조 없음!");
        }

        if (floorText)
            floorText.text = $"{floor:00}층";

        Debug.Log("[MapSelectUI] FloorText 세팅됨: " + floorText?.text);

        Time.timeScale = 0f;
        Refresh();
    }

    public void Close()
    {
        if (panel) panel.SetActive(false);
        if (Time.timeScale == 0f) Time.timeScale = 1f;
    }

    // 현재 위치에서 바로 다음 노드만 선택 가능 + 이미 지난 노드 금지
    public bool CanEnter(string nodeId)
    {
        if (nodeId == currentNodeId) return false;            // 자기 자신 금지
        if (cleared.Contains(nodeId)) return false;         // 지난 노드 금지
        if (!adj.TryGetValue(currentNodeId, out var nexts)) return false;
        return nexts.Contains(nodeId);                        // 바로 다음만 허용
    }

    public void SelectNode(MapNodeButton node)
    {
        // 1) 현재 노드 클리어 처리
        cleared.Add(currentNodeId);

        // 2) 실제 맵 로드
        DungeonMapManager.Instance.LoadMap(node.type, node.prefabIndex, node.spawnId);

        // 3) 현재 위치 이동 + 방문 표시
        currentNodeId = node.nodeId;
        visited.Add(currentNodeId);

        Time.timeScale = 1f;

        // 4) 지도 닫기
        Close();
    }

    public void ResetProgress(string newStartId = null)
    {
        cleared.Clear();
        visited.Clear();
        currentNodeId = string.IsNullOrEmpty(newStartId) ? startNodeId : newStartId;
        visited.Add(currentNodeId);
        Refresh();
    }

    // === 내부: UI 갱신 ===
    void OnEnable() { Refresh(); }

    void Refresh()
    {
        Debug.Log("=== [MapSelectUI.Refresh] 시작 === nodeMap.Count=" + (nodeMap?.Count ?? -1));

        foreach (var kv in nodeMap)
        {
            string id = kv.Key;
            var n = kv.Value;

            if (n == null)
            {
                Debug.LogError("[MapSelectUI] nodeMap[" + id + "] 이 null입니다!");
                continue;
            }

            bool isCurrent = (id == currentNodeId);
            bool isCleared = cleared.Contains(id);
            bool canGoNext = CanEnter(id);

            Debug.Log($"[MapSelectUI.Refresh] 처리중: id={id}, isCurrent={isCurrent}, isCleared={isCleared}, canGoNext={canGoNext}");

            n.SetCurrent(isCurrent);
            n.SetCleared(isCleared);

            bool interactable = (!isCurrent && canGoNext);
            n.SetInteractable(interactable);
        }
    }

    public MapType GetCurrentNodeTypeSafe()
    {
        if (nodeMap != null && nodeMap.TryGetValue(currentNodeId, out var btn) && btn != null)
            return btn.type;
        return MapType.Normal;
    }

    public bool HasNextFromCurrent()
    {
        if (!adj.TryGetValue(currentNodeId, out var nexts)) return false;

        foreach (var id in nexts)
            if (!cleared.Contains(id)) return true;
        return false;
    }
}

[System.Serializable]
public struct Edge
{
    public string from;
    public string to;
}
