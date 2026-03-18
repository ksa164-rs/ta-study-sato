using UnityEngine;

public class GunTracer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform muzzlePoint;
    [SerializeField] GameObject tracerPrefab;

    public void Play()
    {
        if (tracerPrefab == null || muzzlePoint == null)
            return;

        GameObject tracerObj = Instantiate(
            tracerPrefab,
            muzzlePoint.position,
            muzzlePoint.rotation
        );

        TracerEffect tracer = tracerObj.GetComponent<TracerEffect>();

        if (tracer != null)
        {
            tracer.Init(
                muzzlePoint.position,
                muzzlePoint.position + muzzlePoint.forward * 50f
            );
        }
    }
}