using UnityEngine;

public class GunTracer : MonoBehaviour
{
    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Init(Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        Destroy(gameObject, 0.05f);
    }
}