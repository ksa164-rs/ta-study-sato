using UnityEngine;

public class ShootScript : MonoBehaviour
{
    enum FireMode
    {
        Semi,
        Auto
    }

    [Header("Gun Settings")]
    [SerializeField] FireMode fireMode;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform muzzle;

    [SerializeField] float recoil = 0.3f;
    [SerializeField] float fireRate = 10f;

    CameraLook camLook;

    float nextFireTime;

    void Start()
    {
        camLook = GetComponentInChildren<CameraLook>();
    }

    void Update()
    {
        switch (fireMode)
        {
            case FireMode.Semi:
                if (Input.GetMouseButtonDown(0))
                {
                    TryShoot();
                }
                break;

            case FireMode.Auto:
                if (Input.GetMouseButton(0))
                {
                    TryShoot();
                }
                break;
        }
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + 1f / fireRate;

        Shoot();
    }

    void Shoot()
{
    Debug.Log("Shoot");

    camLook.AddRecoil(recoil, 0f);

    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 100f))
    {
        Debug.Log("Hit: " + hit.collider.name);
    }
}

}