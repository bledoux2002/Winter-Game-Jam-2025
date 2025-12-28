using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private InputAction attackAction;

    public int damage = 10;
    public float fireRate = 0.2f;

    float nextAttackTime = 0f;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (attackAction.ReadValue<float>() != 0f && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + fireRate;

        // hitscan example
        if (Physics.Raycast(Camera.main.transform.position,
                             Camera.main.transform.forward,
                             out RaycastHit hit,
                             100f))
        {
            hit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}
