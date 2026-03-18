using UnityEngine;
using System.Collections;

public class GunAmmo : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] int magazineSize = 30;
    [SerializeField] int currentAmmo = 30;
    [SerializeField] int reserveAmmo = 90;

    [Header("Reload")]
    [SerializeField] float reloadTime = 2f;

    bool isReloading = false;
    bool infiniteAmmo = false;

    // =========================
    // 射撃可能か
    // =========================
    public bool CanShoot()
    {
        return currentAmmo > 0 && !isReloading;
    }

    // =========================
    // 弾消費
    // =========================
    public void UseAmmo()
    {
        if (infiniteAmmo) return;

        if (currentAmmo > 0)
            currentAmmo--;
    }

    // =========================
    // リロード開始
    // =========================
    public void Reload()
    {
        if (infiniteAmmo) return;
        if (isReloading) return;
        if (currentAmmo >= magazineSize) return;
        if (reserveAmmo <= 0) return;

        StartCoroutine(ReloadRoutine());
    }

    // =========================
    // リロード処理
    // =========================
    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        Debug.Log("リロード開始");

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;

        Debug.Log("リロード完了");
    }

    // =========================
    // UI用
    // =========================
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetReserveAmmo()
    {
        return reserveAmmo;
    }

    public void SetCurrentAmmo(int value)
    {
        currentAmmo = Mathf.Clamp(value, 0, magazineSize);
    }

    public void SetReserveAmmo(int value)
    {
        reserveAmmo = Mathf.Max(0, value);
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    // =========================
    // デバッグ用
    // =========================
    public void SetInfiniteAmmo(bool value)
    {
        infiniteAmmo = value;
    }

    public bool IsInfiniteAmmo()
    {
        return infiniteAmmo;
    }
}
