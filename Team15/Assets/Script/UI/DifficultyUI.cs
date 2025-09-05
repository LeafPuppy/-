using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    public static DifficultyUI Instance { get; private set; }

    [Header("참조UI")]
    public GameObject panel;
    public Button btnNormal;
    public Button btnHard;

    public string dungeonSceneName = "DungeonScene";

    public bool IsOpen => panel != null && panel.activeSelf;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (panel) panel.SetActive(false);
        btnNormal.onClick.AddListener(() => Pick(Difficulty.Normal));
        btnHard.onClick.AddListener(() => Pick(Difficulty.Hard));
    }

    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Pick(Difficulty diff)
    {
        // 선택 저장
        if (GameState.Instance == null)
        {
            new GameObject("GameState").AddComponent<GameState>();
        }
        GameState.Instance.currentDifficulty = diff;
        GameState.Instance.OnDungeonEnter();

        // 입장
        Time.timeScale = 1f;

        SceneLoadManager.Instance.ChangeScene(dungeonSceneName, () =>
        {
            AudioManager.Instance.BGMSource.clip = ResourceManager.Instance.LoadAsset<AudioClip>("MapSelectBGM", eAssetType.Audio, eCategoryType.BGM);
            AudioManager.Instance.BGMSource.Play();
        });
    }
}
