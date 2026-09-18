using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float nextAttackTime;

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponentInParent<PlayerHealth>();

            if (playerHealth == null)
                playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        playerHealth.TakeDamage(damage);
    }
}