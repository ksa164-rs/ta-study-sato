using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode
    {
        Semi,
        Auto
    }

    [Header("References")]
    GunAmmo ammo;
    GunRaycast raycastSystem;
    GunEffects effects;
    GunRecoil recoil;
    GunTracer tracer;

    [Header("Fire Settings")]
    [SerializeField] FireMode fireMode = FireMode.Auto;
    [SerializeField] float fireRate = 5f;

    float nextFireTime;

    void Awake()
    {
        ammo = GetComponent<GunAmmo>();
        raycastSystem = GetComponent<GunRaycast>();
        effects = GetComponent<GunEffects>();
        recoil = GetComponent<GunRecoil>();
        tracer = GetComponent<GunTracer>();
    }

    void Update()
    {
        HandleShoot();
        HandleReload();
    }

    void HandleShoot()
    {
        if (ammo == null) return;
        if (ammo.IsReloading()) return;

        if (!ammo.CanShoot())
        {
            ammo.Reload();
            return;
        }

        switch (fireMode)
        {
            case FireMode.Semi:
                if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    Shoot();
                }
                break;

            case FireMode.Auto:
                if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    Shoot();
                }
                break;
        }
    }

    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammo != null)
                ammo.Reload();
        }
    }

    void Shoot()
    {
        if (ammo == null || raycastSystem == null || effects == null || tracer == null || recoil == null)
        {
            Debug.LogError("参照が足りない！");
            return;
        }

        ammo.UseAmmo();

        RaycastHit hit;
        bool didHit = raycastSystem.Shoot(out hit);

        effects.PlayMuzzleFlash();
        tracer.Play();

        if (didHit)
        {
            effects.SpawnHitEffect(hit);
            effects.SpawnBulletHole(hit);

            //  ここが修正の本体（親からTarget取得）
            Target target = hit.collider.GetComponentInParent<Target>();

            if (target != null)
            {
                Debug.Log("ダメージ入った！");
                target.TakeDamage(1);
            }
        }

        recoil.AddRecoil();
    }

    public int GetCurrentAmmo()
    {
        return ammo != null ? ammo.GetCurrentAmmo() : 0;
    }

    public int GetReserveAmmo()
    {
        return ammo != null ? ammo.GetReserveAmmo() : 0;
    }
}