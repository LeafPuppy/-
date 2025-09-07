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