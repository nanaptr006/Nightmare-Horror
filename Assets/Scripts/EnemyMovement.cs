using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAttack))]
public class EnemyMovement : MonoBehaviour
{
    Transform player; // Reference to the player's position.
    PlayerHealth playerHealth; // Reference to the player's health.
    EnemyHealth enemyHealth; // Reference to this enemy's health.
    NavMeshAgent nav; // Reference to the nav mesh agent.
    Animator anim;
    [SerializeField] private float detectionDistance = 7f;
    [SerializeField] private int escapeHealthThreshold = 20;
                               //implementing FSM
    public enum States
    {
        idle,
        chase,
        escape
    }
    States _state = States.idle;

    void Start()
    {
        //need to be aware of player health
        playerHealth = player.GetComponent<PlayerHealth>();
    }
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

    void Idling()
    { //just stay put
        anim.SetBool("IsChasing", false);
        nav.enabled = false;
    }
    void Chase() //chase player
    {
        anim.SetBool("IsChasing", true);
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.enabled = true;
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
    void Escape()
    {
        anim.SetBool("IsChasing", true);
        nav.enabled = true;
        // turn away from the player
        transform.rotation = Quaternion.LookRotation(transform.position - player.position);
        Vector3 runTo = transform.position + transform.forward * 10;
        nav.SetDestination(runTo);
    }
    void Update()
    {
        ChangeStates();
        switch (_state)
        {
            case States.idle: Idling(); break;
            case States.chase: Chase(); break;
            case States.escape: Escape(); break;
        }
    }

    void ChangeStates()
    {
        //calculate the distnce btw player and zombunny
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //if player is dead, stop chasing
        if (playerHealth.currentHealth <= 0)
        {
            _state = States.idle; return;
        }
        if (distance < detectionDistance)
        { //player in the zone,
            if (enemyHealth.currentHealth <= escapeHealthThreshold)
            { // low health, escape!
              _state = States.escape; return;
            }
            else
            {
                _state = States.chase; return;
            }
        }
        else
        { //player out the zone
          _state = States.idle; return;
        }
        }
    }
    // void Update()
    // {
    //     bool playerIsDead = playerHealth.currentHealth <= 0;
    //     anim.SetBool("PlayerDead", playerIsDead);

    //     if (enemyHealth.currentHealth <= 0)
    //     {
    //         nav.enabled = false;
    //     }
    //     else if (playerIsDead)
    //     {
    //         nav.isStopped = true;
    //     }
    //     else
    //     {
    //         nav.isStopped = false;
    //         nav.SetDestination(player.position);
    //         anim.SetBool("IsWalking", nav.velocity.magnitude > 0.1f);
    //         return;
    //     }

    //     anim.SetBool("IsWalking", false);
    // }


