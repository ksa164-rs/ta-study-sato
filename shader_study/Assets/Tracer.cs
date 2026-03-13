using UnityEngine;

public class Tracer : MonoBehaviour
{
    Vector3 target;
    float speed = 300f;

    public void Init(Vector3 endPoint)
    {
        target = endPoint;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}