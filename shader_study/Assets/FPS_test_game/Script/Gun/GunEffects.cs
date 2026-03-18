using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 銃エフェクト管理
/// </summary>
public class GunEffects : MonoBehaviour
{
    [SerializeField] Transform muzzlePoint;

    [Header("Effects")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject tracer;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject bulletHole;

    [Header("Bullet Holes")]
    [SerializeField] int maxBulletHoles = 50;

    Queue<GameObject> bulletHoles = new Queue<GameObject>();

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash == null || muzzlePoint == null)
            return;

        GameObject flash = Instantiate(
            muzzleFlash,
            muzzlePoint.position,
            muzzlePoint.rotation,
            muzzlePoint
        );

        Destroy(flash, 0.05f);
    }

    public void SpawnTracer(Vector3 endPoint)
    {
        if (tracer == null || muzzlePoint == null)
            return;

        Vector3 startPos = muzzlePoint.position;

        GameObject tracerObj = Instantiate(tracer);

        TracerEffect tracerComponent = tracerObj.GetComponent<TracerEffect>();

        if (tracerComponent != null)
            tracerComponent.Init(startPos, endPoint);
    }

    public void SpawnTracerFrom(Vector3 startPos, Vector3 endPoint)
    {
        if (tracer == null)
            return;

        GameObject tracerObj = Instantiate(tracer);

        TracerEffect tracerComponent = tracerObj.GetComponent<TracerEffect>();

        if (tracerComponent != null)
            tracerComponent.Init(startPos, endPoint);
    }

    public void SpawnHitEffect(RaycastHit hit)
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

    public void SpawnBulletHole(RaycastHit hit)
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
                Destroy(oldHole);
        }
    }
}