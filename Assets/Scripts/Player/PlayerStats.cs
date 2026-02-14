using PurrNet;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyKnockback))]
public class PlayerStats : NetworkBehaviour, IDamageable, IKnockbackable
{
    [Header("Health settings")]
    [SerializeField] float maxHealth = 100;
    private float currentHealth;
    private bool isDead = false;
    public bool IsDead => isDead;

    [Header("Movement settings")]
    public float movementSpeed = 3;
    public float sprintMultiplier = 1.5f;

    [Header("Combat settings")]

    [SerializeField] private float knockbackDuration = 0.1f;
    public float KnockbackDuration => knockbackDuration;
    private Animator animator;
    private EnemyKnockback knockback;
    private CharacterController characterController;
    private EnemyHitFlash hitFlash;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        knockback = GetComponent<EnemyKnockback>();
        characterController = GetComponent<CharacterController>();
        hitFlash = GetComponent<EnemyHitFlash>();
    }

    [ObserversRpc]
    public void RpcOnHit(DamageInfo damageInfo)
    {
        hitFlash.Play();
        knockback.Apply(damageInfo.direction, damageInfo.force, characterController, knockbackDuration);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        float damageAmount = damageInfo.damage;
        if (isDead)
            return;

        currentHealth -= damageAmount;

        RpcOnHit(damageInfo);

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}