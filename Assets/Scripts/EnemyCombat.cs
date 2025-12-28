using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyCombat : MonoBehaviour
{
    public AudioSource FireAudio;

    public int damage = 10;
    public float fireRate = 2f;
    private float nextAttackTime = 0f;
    public LayerMask playerLayerMask;

    public void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + fireRate;

        FireAudio.Play();

        if (Physics.Raycast(transform.position + Vector3.up,
                             transform.forward,
                             out RaycastHit hit,
                             100f, playerLayerMask))
        {
            Debug.Log("Hit");
            hit.collider.GetComponent<HealthManager>()?.TakeDamage(damage);
        }
    }
}
