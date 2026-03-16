#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class FPSDebugTool : EditorWindow
{
    GameObject enemyPrefab;
    Transform spawnPoint;

    bool infiniteAmmo;

    GunAmmo gun;   // キャッシュ

    [MenuItem("Tools/FPS Debug Tool")]
    static void OpenWindow()
    {
        GetWindow<FPSDebugTool>("FPS Debug");
    }

    void OnGUI()
    {
        FindGunIfNeeded();

        DrawEnemySection();
        GUILayout.Space(10);
        DrawPlayerSection();
        GUILayout.Space(10);
        DrawEnemyUtility();
    }

    /* =============================
     * Utility
     * =============================*/

    void FindGunIfNeeded()
    {
        if (gun == null)
            gun = Object.FindFirstObjectByType<GunAmmo>();
    }

    /* =============================
     * UI Sections
     * =============================*/

    void DrawEnemySection()
    {
        GUILayout.Label("Enemy Spawn", EditorStyles.boldLabel);

        enemyPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Enemy Prefab",
            enemyPrefab,
            typeof(GameObject),
            false);

        spawnPoint = (Transform)EditorGUILayout.ObjectField(
            "Spawn Point",
            spawnPoint,
            typeof(Transform),
            true);

        if (GUILayout.Button("Spawn Enemy"))
        {
            SpawnEnemy();
        }
    }

    void DrawPlayerSection()
    {
        GUILayout.Label("Player Debug", EditorStyles.boldLabel);

        infiniteAmmo = EditorGUILayout.Toggle("Infinite Ammo", infiniteAmmo);

        if (GUILayout.Button("Apply Ammo Setting"))
        {
            ApplyAmmoSetting();
        }
    }

    void DrawEnemyUtility()
    {
        if (GUILayout.Button("Delete All Enemies"))
        {
            DeleteEnemies();
        }
    }

    /* =============================
     * Functions
     * =============================*/

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("EnemyPrefab or SpawnPoint is missing.");
            return;
        }

        GameObject enemy = (GameObject)PrefabUtility.InstantiatePrefab(enemyPrefab);

        Undo.RegisterCreatedObjectUndo(enemy, "Spawn Enemy");

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = Quaternion.identity;
    }

    void ApplyAmmoSetting()
    {
        if (gun == null)
        {
            Debug.LogWarning("GunAmmo not found in scene.");
            return;
        }

        gun.SetInfiniteAmmo(infiniteAmmo);
        Debug.Log("Infinite Ammo: " + infiniteAmmo);
    }

    void DeleteEnemies()
    {
        Target[] enemies = Object.FindObjectsByType<Target>(FindObjectsSortMode.None);

        foreach (Target enemy in enemies)
        {
            Undo.DestroyObjectImmediate(enemy.gameObject);
        }

        Debug.Log("All enemies deleted");
    }
}
#endif