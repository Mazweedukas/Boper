using PurrNet;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Animator))]
public class EnemyCombat : NetworkBehaviour
{
    private float nextAttackTime = 0f;
    private EnemyStats stats;
    private EnemyAnimationController animator;
    [SerializeField] private Transform attackOrigin;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        animator = GetComponent<EnemyAnimationController>();
    }

    public void TryAttack()
    {
        if (!isServer) return;
        if (Time.time >= nextAttackTime)
        {
            animator.PlayAttack();
            nextAttackTime = Time.time + stats.attackCooldown;
        }
    }

    //Called by animation event
    public void DealDamage()
    {
        if (!isServer) return;
        Collider[] hits = Physics.OverlapSphere(
            attackOrigin.position + attackOrigin.forward * stats.attackRange,
            stats.attackRadius
        );

        foreach (var h in hits)
        {
            if (!h.CompareTag("Player"))
                continue;

            Vector3 targetPoint = h.ClosestPoint(transform.position);
            Vector3 direction = (targetPoint - transform.position).normalized;

            DamageInfo info = new()
            {
                damage = stats.attackDamage,
                hitPoint = targetPoint,
                direction = direction,
                force = stats.force,
                source = gameObject
            };

            h.GetComponentInParent<IDamageable>()?.TakeDamage(info);

            SurfaceMaterial surface = h.GetComponentInParent<SurfaceMaterial>();
            SurfaceType type = surface != null ? surface.surfaceType : SurfaceType.Metal;

            // HitEffectManager.Instance.Spawn(type, targetPoint, -direction);
        }
    }
    private void OnDrawGizmosSelected()
    {
        EnemyStats s = GetComponent<EnemyStats>();
        if (s == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position + attackOrigin.forward * s.attackRange, s.attackRadius);
    }
}