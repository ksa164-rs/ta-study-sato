using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Canvas canvas;
    [SerializeField] float visibleTime = 3f;

    float timer;

    void Update()
    {
        if (canvas.enabled)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                canvas.enabled = false;
            }
        }
    }

    public void UpdateHP(float percent)
    {
        slider.value = percent;

        canvas.enabled = true;
        timer = visibleTime;
    }
}