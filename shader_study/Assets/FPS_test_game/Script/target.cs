using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int maxHP = 3;
    int currentHP;

    [SerializeField] EnemyHPUI hpUI;

    void Start()
    {
        currentHP = maxHP;

        if (hpUI != null)
            hpUI.SetMaxHP(maxHP);
            hpUI.UpdateHP(currentHP);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("ダメージ入った");

        currentHP -= damage;

        Debug.Log("現在HP: " + currentHP);

        if (hpUI != null)
            hpUI.UpdateHP(currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("DEAD");
        Destroy(gameObject);
    }
}