using Unity.VisualScripting;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public float range;
    public float damage;
    //Force to be applied to target when hit
    public float force;

    public Transform origin;

    public HitResult Attack(int layerMask)
    {
        Ray ray = new Ray(origin.position, origin.forward);

        HitResult result = new()
        {
            origin = ray.origin,
            hitPoint = ray.origin + ray.direction * range,
            normal = -ray.direction,
            hit = false,
            collider = null,
            surfaceMaterial = null
        };

        if (Physics.Raycast(ray, out var hit, range, layerMask))
        {
            result.hit = true;
            result.hitPoint = hit.point;
            result.normal = hit.normal;
            result.collider = hit.collider;
            result.surfaceMaterial = hit.collider.GetComponentInParent<SurfaceMaterial>();

            hit.collider.GetComponent<IDamageable>()?.TakeDamage(damageInfo: new DamageInfo
            {
                damage = damage,
                hitPoint = hit.point,
                direction = ray.direction,
                force = force,
                source = gameObject
            });
        }

        return result;
    }
}