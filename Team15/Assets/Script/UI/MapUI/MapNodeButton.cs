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

    [Header("Marker Offset")]
    [SerializeField] Vector2 currentMarkerOffset = Vector2.zero;
    [SerializeField] Vector2 clearedMarkerOffset = Vector2.zero;
    [SerializeField] bool bringMarkersToFront = true;

    Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(OnClick);

        if (!currentMarker) currentMarker = transform.Find("CurrentMark")?.gameObject;
        if (!clearedMark) clearedMark = transform.Find("ClearMark")?.gameObject;

        if (currentMarker) currentMarker.SetActive(false);
        if (clearedMark) clearedMark.SetActive(false);

        DisableRaycast(currentMarker);
        DisableRaycast(clearedMark);
    }

    void DisableRaycast(GameObject g)
    {
        if (!g) return;
        var img = g.GetComponent<Image>();
        if (img) img.raycastTarget = false;
    }

    void OnClick()
    {
        if (!MapSelectUI.Instance) return;
        if (!MapSelectUI.Instance.CanEnter(nodeId)) return;
        MapSelectUI.Instance.SelectNode(this);
    }

    public void SetInteractable(bool v)
    {
        if (btn) btn.interactable = v;

        if (lockOverlay != null)
        {
            lockOverlay.alpha = v ? 0f : 0.6f;
            lockOverlay.blocksRaycasts = !v;
        }
    }

    public void SetCurrent(bool v)
    {
        if (!currentMarker) return;
        currentMarker.SetActive(v);
        if (!v) return;

        var mrt = currentMarker.transform as RectTransform;
        var prt = transform as RectTransform;
        if (mrt != null && prt != null)
        {
            mrt.anchorMin = mrt.anchorMax = new Vector2(0.5f, 0.5f);
            mrt.pivot = new Vector2(0.5f, 0.5f);
            mrt.anchoredPosition = currentMarkerOffset;
            mrt.localScale = Vector3.one;
        }

        if (bringMarkersToFront)
            currentMarker.transform.SetAsLastSibling();
    }

    public void SetCleared(bool v)
    {
        if (!clearedMark) return;
        clearedMark.SetActive(v);
        if (!v) return;

        var crt = clearedMark.transform as RectTransform;
        var prt = transform as RectTransform;
        if (crt != null && prt != null)
        {
            crt.anchorMin = crt.anchorMax = new Vector2(0.5f, 0.5f);
            crt.pivot = new Vector2(0.5f, 0.5f);
            crt.anchoredPosition = clearedMarkerOffset;
            crt.localScale = Vector3.one;
        }

        if (bringMarkersToFront)
            clearedMark.transform.SetAsLastSibling();
    }
}
