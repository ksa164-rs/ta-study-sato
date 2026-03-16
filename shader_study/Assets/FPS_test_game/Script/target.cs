using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int health;

    EnemyHPUI hpUI;

    void Start()
    {
        health = maxHealth;
        hpUI = GetComponentInChildren<EnemyHPUI>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        float percent = (float)health / maxHealth;
        hpUI.UpdateHP(percent);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}