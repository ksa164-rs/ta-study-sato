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

    Queue<GameObject> bulletHoles =
        new Queue<GameObject>();

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash == null)
            return;

        GameObject flash =
            Instantiate(
                muzzleFlash,
                muzzlePoint.position,
                muzzlePoint.rotation
            );

        Destroy(flash, 0.2f);
    }

    public void SpawnTracer(Vector3 endPoint)
    {
        if (tracer == null)
            return;

        GameObject tracerObj =
            Instantiate(
                tracer,
                muzzlePoint.position,
                Quaternion.identity
            );

        Tracer tracerComponent =
            tracerObj.GetComponent<Tracer>();

        if (tracerComponent != null)
            tracerComponent.Init(endPoint);

        Destroy(tracerObj, 1f);
    }

    public void SpawnHitEffect(RaycastHit hit)
    {
        if (hitEffect == null)
            return;

        GameObject effect =
            Instantiate(
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

        GameObject hole =
            Instantiate(
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