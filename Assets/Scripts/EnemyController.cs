using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager GameManager;
    private GameObject player;

    public float viewDistance = 20f;
    public int damage = 10;
    public float fireRate = 2f;
    private float nextAttackTime = 0f;

    void Start()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= viewDistance)
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
        if (Physics.Raycast(transform.position,
                             transform.forward,
                             out RaycastHit hit,
                             100f))
        {
            hit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}
