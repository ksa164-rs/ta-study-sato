using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.forward = cam.forward;
    }
}