using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float attackRange;
    [SerializeField] private float damage;
    [SerializeField] private float force;
    [SerializeField] private Transform attackOrigin;
    public void Attack(int layerMask)
    {
        Collider[] hits = Physics.OverlapSphere(attackOrigin.position + attackOrigin.forward * attackRange, radius, layerMask);
        foreach (var hit in hits)
        {
            DamageInfo info = new DamageInfo
            {
                damage = damage,
                hitPoint = hit.ClosestPoint(attackOrigin.position),
                direction = (hit.transform.position - attackOrigin.position).normalized,
                force = force,
                source = gameObject
            };

            hit.GetComponent<IDamageable>()?.TakeDamage(info);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position + attackOrigin.forward * attackRange, radius);
    }


}