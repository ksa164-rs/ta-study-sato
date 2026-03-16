using UnityEngine;

/// <summary>
/// リコイル管理
/// </summary>
public class GunRecoil : MonoBehaviour
{
    [SerializeField] CameraLook camLook;

    [SerializeField] Vector2[] recoilPattern =
    {
        new Vector2(0.08f,0.65f),
        new Vector2(-0.06f,0.75f),
        new Vector2(0.10f,0.85f),
        new Vector2(-0.08f,0.95f),
        new Vector2(0.12f,1.0f),
        new Vector2(-0.10f,1.05f)
    };

    int recoilIndex;

    public void ApplyRecoil()
    {
        if (camLook == null || recoilPattern.Length == 0)
            return;

        Vector2 recoil = recoilPattern[recoilIndex];

        camLook.AddRecoil(recoil.y, recoil.x);

        recoilIndex = Mathf.Min(
            recoilIndex + 1,
            recoilPattern.Length - 1
        );
    }

    public void ResetRecoil()
    {
        recoilIndex = 0;
    }
}