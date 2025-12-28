using UnityEngine;

public class Health : MonoBehaviour
{

    private GameManager GameManager;

    public int maxHealth = 100;
    public int HP { get; private set; }

    public int maxArmor = 0;
    public int Armor { get; private set; }

    void Awake()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        HP = maxHealth;
        Armor = 0;
    }

    public void TakeDamage(int damage)
    {
        int armorAbsorb = Mathf.Min(Armor, damage);
        Armor -= armorAbsorb;
        damage -= armorAbsorb;

        HP -= damage;

        Debug.Log($"Health: {HP}");

        if (gameObject.GetComponent<PlayerController>())
            GameManager.UpdateUI();

        if (HP <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
