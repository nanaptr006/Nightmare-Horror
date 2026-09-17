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
    private bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<playerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();

        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        Debug.Log("Health: " + currentHealth);

        if (playerAudio != null)
            playerAudio.Play();

        if (currentHealth == 0)
            Death();
    }

    private void Death()
    {
        isDead = true;
        playerShooting.DisableEffects();
        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}