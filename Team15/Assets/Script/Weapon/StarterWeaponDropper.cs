using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterWeaponDropper : Singleton<StarterWeaponDropper>
{
    [Header("LifeCycle")]
    [SerializeField] private bool destroyOnLoad = false;
    protected override bool isDestroy => destroyOnLoad;

    [Header("Starter Weapon Prefabs")]
    public EquipableObject swordPrefab;
    public EquipableObject bowPrefab;
    public EquipableObject staffPrefab;

    [Header("Drop Settings")]
    public Vector2 dropOffset = new Vector2(0.6f, 0.2f);
    public float dropImpulse = 2.5f;
    public float dropTorque = 5f;

    private readonly List<GameObject> spawnedByThis = new();

    public void DropNew(StarterWeaponKind kind)
    {
        if (kind == StarterWeaponKind.None) return;

        var player = FindObjectOfType<PlayerController>();
        if (!player)
        {
            Debug.LogWarning("[StarterWeaponDropper] Player 없음");
            return;
        }

        CleanupSpawned();

        var holder = player.weaponHolder;
        if (holder && holder.childCount > 0)
        {
            Destroy(holder.GetChild(0).gameObject);
        }

        var prefab = GetPrefab(kind);
        if (!prefab)
        {
            Debug.LogWarning($"[StarterWeaponDropper] {kind} 프리팹 미지정");
            return;
        }

        var dropPos = (Vector2)player.transform.position + dropOffset;
        var inst = Instantiate(prefab, dropPos, Quaternion.identity);

        inst.isEquipable = true;
        if (inst.objectUI) inst.objectUI.SetActive(true);

        var rb2d = inst.GetComponent<Rigidbody2D>();
        if (rb2d)
        {
            rb2d.simulated = true;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.AddForce(Vector2.up * dropImpulse, ForceMode2D.Impulse);
            rb2d.AddTorque(dropTorque, ForceMode2D.Impulse);
        }

        spawnedByThis.Add(inst.gameObject);
    }

    public void CleanupSpawned()
    {
        for (int i = spawnedByThis.Count - 1; i >= 0; i--)
        {
            if (spawnedByThis[i]) Destroy(spawnedByThis[i]);
        }
        spawnedByThis.Clear();
    }

    public void EquipCurrentToPlayer()
    {
        var gs = GameState.Instance;
        if (gs == null) return;

        var kind = gs.currentStarterWeapon;
        if (kind == StarterWeaponKind.None) return;

        var player = FindObjectOfType<PlayerController>();
        if (!player || !player.weaponHolder) return;

        // 손에 기존 무기 제거
        var holder = player.weaponHolder;
        if (holder.childCount > 0)
            Destroy(holder.GetChild(0).gameObject);

        // 프리팹 인스턴스 생성 후 장착
        var prefab = GetPrefab(kind);
        if (!prefab) return;

        var inst = Instantiate(prefab);

        // 즉시 장착 상태
        inst.isEquipable = false;
        if (inst.objectUI) inst.objectUI.SetActive(false);

        inst.transform.SetParent(holder);
        inst.transform.localPosition = Vector3.zero;

        // 좌우 반전/회전 정리
        float flip = Mathf.Sign(player.transform.localScale.x);
        inst.transform.localScale = new Vector3(
            flip * Mathf.Abs(inst.transform.localScale.x),
            inst.transform.localScale.y,
            inst.transform.localScale.z
        );
        inst.transform.localRotation = flip < 0
            ? Quaternion.Euler(0f, 180f, 0f)
            : Quaternion.identity;

        // 물리 비활성
        var rb2d = inst.GetComponent<Rigidbody2D>();
        if (rb2d) rb2d.simulated = false;
    }

    private EquipableObject GetPrefab(StarterWeaponKind k)
    {
        switch (k)
        {
            case StarterWeaponKind.Sword: return swordPrefab;
            case StarterWeaponKind.Bow: return bowPrefab;
            case StarterWeaponKind.Staff: return staffPrefab;
            default: return null;
        }
    }
}