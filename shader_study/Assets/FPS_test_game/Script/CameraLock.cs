using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Look")]
    [SerializeField] float sensitivity = 80f;
    [SerializeField] Camera cam;

    [Header("ADS")]
    [SerializeField] float normalFOV = 60f;
    [SerializeField] float adsFOV = 40f;
    [SerializeField] float adsSpeed = 10f;

    [Header("Recoil")]
    [SerializeField] float recoilReturnSpeed = 10f; // 目標反動が0に戻る速さ
    [SerializeField] float recoilSnappiness = 14f;  // 現在反動が目標反動に追従する速さ

    float xRotation = 0f;
    float yRotation = 0f;

    // 今フレーム実際に見た目へ反映する反動
    Vector2 currentRecoil;

    // 発砲時に加算される目標反動
    Vector2 targetRecoil;

    void Start()
    {
        if (cam == null)
            cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        HandleMouseLook();
        UpdateRecoil();
        HandleCursor();
        HandleADS();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        float finalX = xRotation - currentRecoil.x;
        float finalY = yRotation + currentRecoil.y;

        transform.localRotation = Quaternion.Euler(finalX, 0f, 0f);

        if (transform.parent != null)
        {
            transform.parent.rotation = Quaternion.Euler(0f, finalY, 0f);
        }
    }

    void UpdateRecoil()
    {
        // 目標反動を自然に0へ戻す
        targetRecoil = Vector2.Lerp(
            targetRecoil,
            Vector2.zero,
            recoilReturnSpeed * Time.deltaTime
        );

        // 現在反動を目標反動へ追従させる
        currentRecoil = Vector2.Lerp(
            currentRecoil,
            targetRecoil,
            recoilSnappiness * Time.deltaTime
        );
    }

    void HandleCursor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleADS()
    {
        if (cam == null) return;

        float targetFOV = normalFOV;

        if (Input.GetMouseButton(1))
        {
            targetFOV = adsFOV;
        }

        cam.fieldOfView = Mathf.Lerp(
            cam.fieldOfView,
            targetFOV,
            adsSpeed * Time.deltaTime
        );
    }

    public void AddRecoil(float vertical, float horizontal)
    {
        targetRecoil += new Vector2(vertical, horizontal);
    }
}