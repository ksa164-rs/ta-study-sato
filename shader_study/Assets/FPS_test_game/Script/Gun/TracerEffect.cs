using UnityEngine;

/// <summary>
/// トレーサー見た目用
/// </summary>
public class TracerEffect : MonoBehaviour
{
    [SerializeField] float speed = 200f;
    [SerializeField] float lifeTime = 0.1f;

    Vector3 startPoint;
    Vector3 endPoint;
    bool isInitialized = false;
    float progress = 0f;

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Init(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;

        transform.position = startPoint;
        transform.LookAt(endPoint);

        // 初期位置
        if (lr != null)
        {
            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, startPoint);
        }

        isInitialized = true;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!isInitialized)
            return;

        float distance = Vector3.Distance(startPoint, endPoint);

        if (distance <= 0.001f)
        {
            transform.position = endPoint;
            return;
        }

        progress += (speed / distance) * Time.deltaTime;
        Vector3 currentPos = Vector3.Lerp(startPoint, endPoint, progress);

        transform.position = currentPos;

        // 🔥 ここが超重要
        if (lr != null)
        {
            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, currentPos);
        }

        if (progress >= 1f)
        {
            transform.position = endPoint;
            Destroy(gameObject);
        }
    }
}