using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    [SerializeField] CameraLook cameraLook;
    [SerializeField] float recoilX = 2f;
    [SerializeField] float recoilY = 1f;

    public void AddRecoil()
    {
        if (cameraLook == null)
            return;

        cameraLook.AddRecoil(recoilX, Random.Range(-recoilY, recoilY));
    }
}