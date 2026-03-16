using UnityEngine;

/// <summary>
/// ヒットスキャン処理
/// </summary>
public class GunRaycast : MonoBehaviour
{
    public Vector3 Fire(
        Camera cam,
        Transform muzzlePoint,
        float range,
        float bloom,
        GunEffects effects)
    {
        Vector3 viewport = new Vector3(
            0.5f + Random.Range(-bloom, bloom) * 0.01f,
            0.5f + Random.Range(-bloom, bloom) * 0.01f,
            0f
        );

        Ray cameraRay = cam.ViewportPointToRay(viewport);

        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out RaycastHit cameraHit, range))
            targetPoint = cameraHit.point;
        else
            targetPoint = cameraRay.origin + cameraRay.direction * range;

        Vector3 direction =
            (targetPoint - muzzlePoint.position).normalized;

        Ray muzzleRay =
            new Ray(muzzlePoint.position, direction);

        if (Physics.Raycast(muzzleRay, out RaycastHit hit, range))
        {
            effects.SpawnHitEffect(hit);

            Target target =
                hit.collider.GetComponentInParent<Target>();

            if (target != null)
                target.TakeDamage(1);
            else
                effects.SpawnBulletHole(hit);

            return hit.point;
        }

        return muzzleRay.origin +
               muzzleRay.direction * range;
    }
}