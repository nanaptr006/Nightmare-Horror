using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(playerMovement))]
public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deathClip;

    private Animator anim;
    private AudioSource playerAudio;
    private playerMovement playerMovement;
    private PlayerShooting playerShooting;
    [SerializeField] private HealthBar healthBar;
    private bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<playerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();

        currentHealth = startingHealth;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, startingHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, startingHealth);

        Debug.Log("Health: " + currentHealth);

        if (playerAudio != null)
            playerAudio.Play();

        if (currentHealth == 0)
            Death();
    }

    private void Death()
    {
        isDead = true;
        KillCount.ResetCount();
        playerShooting.DisableEffects();
        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}