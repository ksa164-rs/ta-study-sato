using UnityEngine;
using System.Collections;

/// <summary>
/// 弾管理
/// </summary>
public class GunAmmo : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] int magazineSize = 30;
    [SerializeField] int currentAmmo;
    [SerializeField] int reserveAmmo = 90;
    [SerializeField] float reloadTime = 2f;

    bool isReloading;
    bool infiniteAmmo;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    public bool CanShoot()
    {
        if (isReloading)
            return false;

        if (infiniteAmmo)
            return true;

        if (currentAmmo <= 0)
            return false;

        return true;
    }

    public void ConsumeAmmo()
    {
        if (!infiniteAmmo)
            currentAmmo--;
    }

    public void Reload()
    {
        if (infiniteAmmo) return;
        if (isReloading) return;
        if (currentAmmo == magazineSize) return;
        if (reserveAmmo <= 0) return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
    }

    public void SetInfiniteAmmo(bool value)
    {
        infiniteAmmo = value;
    }
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetReserveAmmo()
    {
        return reserveAmmo;
    }
    public bool IsReloading()
    {
        return isReloading;
    }
}