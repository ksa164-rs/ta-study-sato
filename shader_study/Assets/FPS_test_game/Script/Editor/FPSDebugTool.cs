#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FPSDebugTool : EditorWindow
{
    const string DefaultTargetPrefabPath = "Assets/FPS_test_game/3D/Prefab/Enemy.prefab";

    GunAmmo gunAmmo;
    GameObject targetPrefab;

    bool infiniteAmmo;
    Vector2 randomSpawnRange = new Vector2(10f, 10f);

    [MenuItem("Tools/FPS Debug Tool")]
    static void OpenWindow()
    {
        GetWindow<FPSDebugTool>("FPS Debug");
    }

    void OnGUI()
    {
        FindAmmoIfNeeded();
        FindTargetPrefabIfNeeded();

        GUILayout.Label("Player Debug", EditorStyles.boldLabel);

        if (gunAmmo == null)
        {
            EditorGUILayout.HelpBox("GunAmmo not found in scene", MessageType.Warning);
        }
        else
        {
            DrawAmmoSection();
        }

        GUILayout.Space(10);
        DrawTargetSection();
    }

    void FindAmmoIfNeeded()
    {
        if (gunAmmo == null)
            gunAmmo = Object.FindFirstObjectByType<GunAmmo>();
    }

    void FindTargetPrefabIfNeeded()
    {
        if (targetPrefab == null)
            targetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(DefaultTargetPrefabPath);
    }

    void DrawAmmoSection()
    {
        GUILayout.Label("Ammo", EditorStyles.boldLabel);

        int currentAmmo = gunAmmo.GetCurrentAmmo();
        int reserveAmmo = gunAmmo.GetReserveAmmo();

        int newCurrentAmmo = EditorGUILayout.IntField("Current Ammo", currentAmmo);
        int newReserveAmmo = EditorGUILayout.IntField("Reserve Ammo", reserveAmmo);

        if (newCurrentAmmo != currentAmmo)
        {
            Undo.RecordObject(gunAmmo, "Change Current Ammo");
            gunAmmo.SetCurrentAmmo(newCurrentAmmo);
            EditorUtility.SetDirty(gunAmmo);
        }

        if (newReserveAmmo != reserveAmmo)
        {
            Undo.RecordObject(gunAmmo, "Change Reserve Ammo");
            gunAmmo.SetReserveAmmo(newReserveAmmo);
            EditorUtility.SetDirty(gunAmmo);
        }

        GUILayout.Space(5);

        bool newInfiniteAmmo = EditorGUILayout.Toggle("Infinite Ammo", gunAmmo.IsInfiniteAmmo());

        if (newInfiniteAmmo != infiniteAmmo)
        {
            infiniteAmmo = newInfiniteAmmo;
            gunAmmo.SetInfiniteAmmo(infiniteAmmo);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Reload"))
        {
            gunAmmo.Reload();
        }
    }

    void DrawTargetSection()
    {
        GUILayout.Label("Target Spawn", EditorStyles.boldLabel);

        targetPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Target Prefab",
            targetPrefab,
            typeof(GameObject),
            false
        );

        randomSpawnRange = EditorGUILayout.Vector2Field("Random Range", randomSpawnRange);

        using (new EditorGUI.DisabledScope(targetPrefab == null))
        {
            if (GUILayout.Button("Spawn Target At Origin"))
            {
                SpawnTarget(Vector3.zero);
            }

            if (GUILayout.Button("Spawn Target Randomly"))
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-randomSpawnRange.x, randomSpawnRange.x),
                    0f,
                    Random.Range(-randomSpawnRange.y, randomSpawnRange.y)
                );

                SpawnTarget(randomPosition);
            }

            if (GUILayout.Button("Delete All Enemy Prefabs"))
            {
                DeleteAllEnemyPrefabsInHierarchy();
            }
        }

        if (targetPrefab == null)
        {
            EditorGUILayout.HelpBox("Target prefab is not assigned.", MessageType.Warning);
        }
    }

    void SpawnTarget(Vector3 position)
    {
        GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(targetPrefab);

        if (spawnedObject == null)
        {
            Debug.LogWarning("Failed to spawn target prefab.");
            return;
        }

        Undo.RegisterCreatedObjectUndo(spawnedObject, "Spawn Target");
        spawnedObject.transform.position = position;
        Selection.activeGameObject = spawnedObject;
    }

    void DeleteAllEnemyPrefabsInHierarchy()
    {
        if (targetPrefab == null)
        {
            Debug.LogWarning("Target prefab is not assigned.");
            return;
        }

        GameObject[] sceneObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<GameObject> objectsToDelete = new List<GameObject>();
        int deletedCount = 0;

        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject == null)
                continue;

            GameObject prefabInstanceRoot = PrefabUtility.GetNearestPrefabInstanceRoot(sceneObject);

            if (prefabInstanceRoot == null || prefabInstanceRoot != sceneObject)
                continue;

            GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(sceneObject);

            if (sourcePrefab != targetPrefab)
                continue;

            objectsToDelete.Add(sceneObject);
        }

        foreach (GameObject sceneObject in objectsToDelete)
        {
            if (sceneObject == null)
                continue;

            Undo.DestroyObjectImmediate(sceneObject);
            deletedCount++;
        }

        Debug.Log($"Deleted {deletedCount} Enemy prefab instance(s).");
    }
}
#endif
