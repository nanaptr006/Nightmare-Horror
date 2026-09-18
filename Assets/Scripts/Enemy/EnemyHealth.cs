using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;
	public int currentHealth;
	public float sinkSpeed = 2.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;

	public event System.Action<EnemyHealth> Died;

	Animator anim;
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	CapsuleCollider capsuleCollider;
	bool isDead;
	bool isSinking;


	void Awake()
	{
		anim = GetComponent<Animator>();
		enemyAudio = GetComponent<AudioSource>();
		hitParticles = GetComponentInChildren<ParticleSystem>();
		capsuleCollider = GetComponent<CapsuleCollider>();

		currentHealth = startingHealth;
	}

	public void SetHealth(int health)
	{
		startingHealth = health;
		currentHealth = health;
	}


	void Update()
	{
		if (isSinking)      //If isSinking is true, then make the ZomBunny sink to the bottom
		{
			transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}


	public void TakeDamage(int amount, Vector3 hitPoint)
	{
		if (isDead)
			return;

		enemyAudio.Play();

		currentHealth -= amount;

		// Position the hitParticles to the hitPoint where the ZomBunny is hit
		hitParticles.transform.position = hitPoint;
		// Play the particle effect
		hitParticles.Play();

		if (currentHealth <= 0)
		{
			Death();
		}
	}


	void Death()
	{
		if (isDead)
			return;

		isDead = true;

		capsuleCollider.isTrigger = true;

		anim.SetTrigger("Dead");        //when the enemy is dead, set the trigger called "Dead" in the attached animator to true for a while.

		enemyAudio.clip = deathClip;
		enemyAudio.Play();

		KillCount.AddKill();
		Died?.Invoke(this);

	}


	public void StartSinking()
	{
		// When the ZomBunny starts sinking, it should not be moved by Physics interaction. We then its Rigidbody to be Kinematic.
		GetComponent<Rigidbody>().isKinematic = true;
		isSinking = true;
		GetComponent<EnemyMovement>().enabled = false;
		GetComponent<NavMeshAgent>().enabled = false;
		// The dead ZomBunny will destroy itself within 2 seconds.
		Destroy(gameObject, 1.5f);
	}
}
