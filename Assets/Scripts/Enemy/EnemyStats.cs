using PurrNet;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable, IKnockbackable, IStaggerable
{
    private EnemyHitFlash hitFlash;
    private EnemyKnockback knockback;
    private EnemyHitStun hitStun;
    private Collider enemyCollider;
    private EnemyAnimationController animator;
    private EnemyController controller;
    private CharacterController characterController;

    [SerializeField] private float maxHealth = 100f;
    public float attackDamage = 10f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackRadius = 2f;
    public float force = 0.5f;
    [SerializeField] public float attackCooldown = 3f;
    [SerializeField] private float staggerDuration = 0.1f;
    [SerializeField] private float knockbackDuration = 0.1f;
    public float KnockbackDuration => knockbackDuration;
    public float StaggerDuration => staggerDuration;
    private float currentHealth;
    bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;

        enemyCollider = GetComponent<Collider>();
        knockback = GetComponent<EnemyKnockback>();
        hitStun = GetComponent<EnemyHitStun>();
        hitFlash = GetComponent<EnemyHitFlash>();
        animator = GetComponent<EnemyAnimationController>();
        controller = GetComponent<EnemyController>();
        characterController = GetComponent<CharacterController>();
    }

    [ObserversRpc]
    public void RpcOnHit(DamageInfo damageInfo)
    {
        hitFlash.Play();
        knockback.Apply(damageInfo.direction, damageInfo.force, characterController, knockbackDuration);
        hitStun.Apply(staggerDuration);
        animator.Stagger();

        Debug.Log("RpcOnHit called on " + (isServer ? "SERVER" : "CLIENT"));
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (!isServer) return;
        if (isDead) return;

        RpcOnHit(damageInfo);

        currentHealth -= damageInfo.damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        knockback.Stop();
        hitStun.Stop();

        isDead = true;
        enemyCollider.enabled = false;
        controller.EnterDead();
    }
}
