using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    [SerializeField] Image hpBar;

    int maxHP;

    public void SetMaxHP(int hp)
    {
        maxHP = hp;
    }

    public void UpdateHP(int currentHP)
    {
        if (hpBar == null) return;
        if (maxHP <= 0) return; // ← 保険

        float ratio = (float)currentHP / maxHP;
        hpBar.fillAmount = ratio;
    }
}