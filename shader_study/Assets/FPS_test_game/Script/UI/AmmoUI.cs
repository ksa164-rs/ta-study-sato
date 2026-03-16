using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] Gun gun;

    [Header("Numbers")]
    [SerializeField] Sprite[] numbers;

    [Header("Magazine")]
    [SerializeField] Image mag10;
    [SerializeField] Image mag1;

    [Header("Reserve")]
    [SerializeField] Image res100;
    [SerializeField] Image res10;
    [SerializeField] Image res1;

    void Update()
    {
        if (gun == null)
            return;

        if (numbers == null || numbers.Length < 10)
            return;

        UpdateAmmo(gun.GetCurrentAmmo(), gun.GetReserveAmmo());
    }

    void UpdateAmmo(int mag, int reserve)
    {
        // Magazine
        int m10 = mag / 10;
        int m1 = mag % 10;

        if (mag10 != null)
        {
            mag10.sprite = numbers[m10];
            mag10.enabled = mag >= 10;
        }

        if (mag1 != null)
        {
            mag1.sprite = numbers[m1];
        }

        // Reserve
        int r100 = reserve / 100;
        int r10 = (reserve / 10) % 10;
        int r1 = reserve % 10;

        if (res100 != null)
        {
            res100.sprite = numbers[r100];
            res100.enabled = reserve >= 100;
        }

        if (res10 != null)
        {
            res10.sprite = numbers[r10];
            res10.enabled = reserve >= 10;
        }

        if (res1 != null)
        {
            res1.sprite = numbers[r1];
        }
    }
}