using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class Gun : MonoBehaviour
{
    enum FireMode
    {
        Semi,
        Auto
    }

    /* =============================
     * References
     * =============================*/
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] CameraLook camLook;
    [SerializeField] Transform muzzlePoint;

    /* =============================
     * Gun Settings
     * =============================*/
    [Header("Gun Settings")]
    [SerializeField] FireMode fireMode = FireMode.Auto;
    [SerializeField] float fireRate = 10f;
    [SerializeField] float range = 100f;

    /* =============================
     * Bloom
     * =============================*/
    [Header("Bloom")]
    [SerializeField] float bloom = 0f;
    [SerializeField] float bloomIncrease = 0.15f;
    [SerializeField] float bloomMax = 1.2f;
    [SerializeField] float bloomRecovery = 3f;

    /* =============================
     * Recoil
     * =============================*/
    [Header("Recoil Pattern")]
    [SerializeField] Vector2[] recoilPattern =
    {
        new Vector2(0.08f, 0.65f),
        new Vector2(-0.06f, 0.75f),
        new Vector2(0.10f, 0.85f),
        new Vector2(-0.08f, 0.95f),
        new Vector2(0.12f, 1.00f),
        new Vector2(-0.10f, 1.05f)
    };

    /* =============================
     * Effects
     * =============================*/
    [Header("Effects")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject tracer;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject bulletHole;

    [Header("Bullet Hole Settings")]
    [SerializeField] int maxBulletHoles = 50;

    /* =============================
     * Internal State
     * =============================*/
    int recoilIndex;
    float nextFireTime;

    Queue<GameObject> bulletHoles = new Queue<GameObject>();

    void Update()
    {
        HandleShootInput();
        RecoverBloom();

        if (Input.GetMouseButtonUp(0))
        {
            ResetRecoil();
        }
    }

    /* =============================
     * Input
     * =============================*/
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

    /* =============================
     * Shoot Logic
     * =============================*/
    void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + 1f / fireRate;
        Shoot();
    }

    void Shoot()
    {
        ApplyRecoil();

        bloom = Mathf.Min(bloom + bloomIncrease, bloomMax);

        FireRay();
        PlayMuzzleFlash();
    }

    /* =============================
     * Recoil
     * =============================*/
    void ApplyRecoil()
    {
        if (camLook == null || recoilPattern == null || recoilPattern.Length == 0)
            return;

        Vector2 recoil = recoilPattern[recoilIndex];

        camLook.AddRecoil(recoil.y, recoil.x);

        recoilIndex = Mathf.Min(recoilIndex + 1, recoilPattern.Length - 1);
    }

    public void ResetRecoil()
    {
        recoilIndex = 0;
    }

    /* =============================
     * Bloom
     * =============================*/
    void RecoverBloom()
    {
        bloom = Mathf.MoveTowards(
            bloom,
            0f,
            bloomRecovery * Time.deltaTime
        );
    }

    /* =============================
     * Raycast
     * =============================*/
    void FireRay()
    {
        Vector3 viewport = new Vector3(
            0.5f + Random.Range(-bloom, bloom) * 0.01f,
            0.5f + Random.Range(-bloom, bloom) * 0.01f,
            0f
        );

        Ray cameraRay = cam.ViewportPointToRay(viewport);
        RaycastHit cameraHit;

        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out cameraHit, range))
        {
            targetPoint = cameraHit.point;
        }
        else
        {
            targetPoint = cameraRay.origin + cameraRay.direction * range;
        }

        Vector3 direction = (targetPoint - muzzlePoint.position).normalized;

        Ray muzzleRay = new Ray(muzzlePoint.position, direction);
        RaycastHit hit;

        Debug.DrawRay(muzzleRay.origin, muzzleRay.direction * range, Color.red, 1f);

        Vector3 endPoint;

        if (Physics.Raycast(muzzleRay, out hit, range))
        {
            endPoint = hit.point;

            SpawnHitEffect(hit);

            // ===== 敵なら弾痕を作らない =====
            Target target = hit.collider.GetComponentInParent<Target>();

            if (target != null)
            {
                target.TakeDamage(1);
            }
            else
            {
                SpawnBulletHole(hit);
            }
        }
        else
        {
            endPoint = muzzleRay.origin + muzzleRay.direction * range;
        }

        SpawnTracer(endPoint);
    }

    /* =============================
     * Effects
     * =============================*/
    void PlayMuzzleFlash()
    {
        if (muzzleFlash == null || muzzlePoint == null)
            return;

        GameObject flash = Instantiate(
            muzzleFlash,
            muzzlePoint.position,
            muzzlePoint.rotation
        );

        Destroy(flash, 0.2f);
    }

    void SpawnTracer(Vector3 endPoint)
    {
        if (tracer == null || muzzlePoint == null)
            return;

        GameObject tracerObj = Instantiate(
            tracer,
            muzzlePoint.position,
            Quaternion.identity
        );

        Tracer tracerComponent = tracerObj.GetComponent<Tracer>();
        if (tracerComponent != null)
        {
            tracerComponent.Init(endPoint);
        }

        Destroy(tracerObj, 1f);
    }

    void SpawnHitEffect(RaycastHit hit)
    {
        if (hitEffect == null)
            return;

        GameObject effect = Instantiate(
            hitEffect,
            hit.point,
            Quaternion.LookRotation(hit.normal)
        );

        Destroy(effect, 1f);
    }

    void SpawnBulletHole(RaycastHit hit)
    {
        if (bulletHole == null)
            return;

        GameObject hole = Instantiate(
            bulletHole,
            hit.point + hit.normal * 0.01f,
            Quaternion.LookRotation(hit.normal)
        );

        hole.transform.SetParent(hit.collider.transform);

        bulletHoles.Enqueue(hole);

        if (bulletHoles.Count > maxBulletHoles)
        {
            GameObject oldHole = bulletHoles.Dequeue();

            if (oldHole != null)
            {
                Destroy(oldHole);
            }
        }
    }
}