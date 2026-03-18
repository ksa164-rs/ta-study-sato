using UnityEngine;

public class GunRaycast : MonoBehaviour
{
    [SerializeField] float range = 100f;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;

        // 念のための保険
        if (cam == null)
        {
            Debug.LogError("Camera.main が見つかってない");
        }
    }

    public bool Shoot(out RaycastHit hit)
    {
        Debug.Log("Ray飛んでる"); // ←追加

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit!");
            return true;
        }

        return false;
    }
}