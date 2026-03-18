using UnityEngine;

public class GunManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] GameObject[] weapons; // handgun, rifle, knife

    int currentIndex = 0;

    void Start()
    {
        SetWeapon(0);
    }

    void Update()
    {
        HandleScrollInput();
        HandleNumberInput();
    }

    public Gun GetCurrentGun()
    {
        return weapons[currentIndex].GetComponent<Gun>();
    }
    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            NextWeapon();
        }
        else if (scroll < 0f)
        {
            PrevWeapon();
        }
    }

    void HandleNumberInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetWeapon(2);
    }

    void NextWeapon()
    {
        currentIndex++;
        if (currentIndex >= weapons.Length)
            currentIndex = 0;

        SetWeapon(currentIndex);
    }

    void PrevWeapon()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = weapons.Length - 1;

        SetWeapon(currentIndex);
    }

    void SetWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        currentIndex = index;
    }
}