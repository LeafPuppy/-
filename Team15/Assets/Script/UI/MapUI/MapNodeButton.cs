using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    [Header("ID / 로드 정보")]
    public string nodeId = "Start";
    public MapType type;
    public int prefabIndex = -1;
    public string spawnId = null;

    [Header("표시용")]
    public GameObject currentMarker;
    public GameObject clearedMark;
    public CanvasGroup lockOverlay;

    Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!MapSelectUI.Instance) return;
        if (!MapSelectUI.Instance.CanEnter(nodeId)) return; // 보호
        MapSelectUI.Instance.SelectNode(this);
    }

    // ----- UI 상태 갱신 헬퍼 -----
    public void SetInteractable(bool v)
    {
        if (btn) btn.interactable = v;

        if (lockOverlay != null)
        {
            lockOverlay.alpha = v ? 0f : 0.6f;
            lockOverlay.blocksRaycasts = !v;
        }
    }
    public void SetCurrent(bool v) { if (currentMarker) currentMarker.SetActive(v); }
    public void SetCleared(bool v) { if (clearedMark) clearedMark.SetActive(v); }
}
