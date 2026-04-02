using UnityEngine;
using UnityEngine.Audio;

public class PlayerHealth : LivingEntity
{
    private AudioSource audio;
    public AudioClip hurtClip;
    public AudioClip deathClip;

    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Health = startingHealth;
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead)
        {
            base.OnDamage(damage, hitPoint, hitNormal);
            GameManager.instance.hpBarUpdate(Health / startingHealth);
            audio.PlayOneShot(hurtClip);
        }
    }

    public override void Die()
    {
        if (IsDead)
            return;
        base.Die();
        audio.PlayOneShot(deathClip);
        animator.SetTrigger("Die");
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        GetComponent<Collider>().enabled = false;
        GameManager.instance.GameOver();
    }

    public void RestartLevel()
    {
        GameManager.instance.RestartLevel();
    }
}
