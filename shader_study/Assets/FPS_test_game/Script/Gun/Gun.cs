using UnityEngine;

/// <summary>
/// 銃のメイン制御
/// </summary>
public class Gun : MonoBehaviour
{
    enum FireMode
    {
        Semi,
        Auto
    }

    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Transform muzzlePoint;

    [Header("Modules")]
    [SerializeField] GunAmmo ammo;
    [SerializeField] GunRecoil recoil;
    [SerializeField] GunRaycast raycastSystem;
    [SerializeField] GunEffects effects;

    [Header("Gun Settings")]
    [SerializeField] FireMode fireMode = FireMode.Auto;
    [SerializeField] float fireRate = 10f;
    [SerializeField] float range = 100f;

    [Header("Bloom")]
    [SerializeField] float bloom;
    [SerializeField] float bloomIncrease = 0.15f;
    [SerializeField] float bloomMax = 1.2f;
    [SerializeField] float bloomRecovery = 3f;

    float nextFireTime;

    void Update()
    {
        HandleShootInput();
        RecoverBloom();

        if (Input.GetMouseButtonUp(0))
            recoil.ResetRecoil();

        if (Input.GetKeyDown(KeyCode.R))
            ammo.Reload();
    }

    void HandleShootInput()
    {
        switch (fireMode)
        {
            case FireMode.Semi:
                if (Input.GetMouseButtonDown(0))
                    TryShoot();
                break;

            case FireMode.Auto:
                if (Input.GetMouseButton(0))
                    TryShoot();
                break;
        }
    }

    void TryShoot()
    {
        if (!ammo.CanShoot())
        {
            ammo.Reload();
            return;
        }

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + 1f / fireRate;

        Shoot();
    }

    void Shoot()
    {
        ammo.ConsumeAmmo();

        recoil.ApplyRecoil();

        ApplyBloom();

        Vector3 hitPoint = raycastSystem.Fire(
            cam,
            muzzlePoint,
            range,
            bloom,
            effects
        );

        effects.PlayMuzzleFlash();
        effects.SpawnTracer(hitPoint);
    }

    void ApplyBloom()
    {
        bloom = Mathf.Min(bloom + bloomIncrease, bloomMax);
    }

    void RecoverBloom()
    {
        bloom = Mathf.MoveTowards(
            bloom,
            0f,
            bloomRecovery * Time.deltaTime
        );
    }
}