#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class FPSDebugTool : EditorWindow
{
    GameObject enemyPrefab;
    Transform spawnPoint;

    bool infiniteAmmo;

    GunAmmo gun;

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
     * UI
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

        if (GUILayout.Button("Spawn Enemy Random"))
        {
            SpawnEnemyRandom();
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
        GUILayout.Label("Enemy Utility", EditorStyles.boldLabel);

        if (GUILayout.Button("Delete All Enemies"))
        {
            DeleteEnemies();
        }
    }

    /* =============================
     * Enemy Functions
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

    void SpawnEnemyRandom()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemyPrefab is missing.");
            return;
        }

        Vector3 randomPos = new Vector3(
            Random.Range(-10f, 10f),
            0,
            Random.Range(-10f, 10f)
        );

        GameObject enemy = (GameObject)PrefabUtility.InstantiatePrefab(enemyPrefab);

        Undo.RegisterCreatedObjectUndo(enemy, "Spawn Enemy Random");

        enemy.transform.position = randomPos;
        enemy.transform.rotation = Quaternion.identity;
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

    /* =============================
     * Player Functions
     * =============================*/

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
}
#endif