using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Transform player; // Reference to the player's position.
    PlayerHealth playerHealth; // Reference to the player's health.
    EnemyHealth enemyHealth; // Reference to this enemy's health.
    NavMeshAgent nav; // Reference to the nav mesh agent.
    Animator anim;
    void Awake()
    {
        // Set up the references.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bool playerIsDead = playerHealth.currentHealth <= 0;
        anim.SetBool("PlayerDead", playerIsDead);

        if (enemyHealth.currentHealth <= 0)
        {
            nav.enabled = false;
        }
        else if (playerIsDead)
        {
            nav.isStopped = true;
        }
        else
        {
            nav.isStopped = false;
            nav.SetDestination(player.position);
            anim.SetBool("IsWalking", nav.velocity.magnitude > 0.1f);
            return;
        }

        anim.SetBool("IsWalking", false);
    }
}
