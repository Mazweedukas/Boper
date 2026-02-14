using Unity.Mathematics;
using UnityEngine;

public class WeaponVFX : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public TrailRenderer tracerPrefab;

    public void PlayShot(HitResult hit)
    {
        if (muzzleFlash != null)
        {
            ParticleSystem flash =
                Instantiate(muzzleFlash, muzzleFlash.transform.position, muzzleFlash.transform.rotation);

            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }

        if (tracerPrefab != null)
        {
            TrailRenderer tracer = Instantiate(tracerPrefab, hit.origin, quaternion.identity);
            tracer.AddPosition(hit.hitPoint);
        }

    }

    public void PlayHitEffect(HitResult hit)
    {
        // if (hit.collider != null)
        // {
        //     SurfaceMaterial surfaceMaterial = hit.collider.GetComponentInParent<SurfaceMaterial>();
        //     if (surfaceMaterial != null)
        //         HitEffectManager.Instance.Spawn(surfaceMaterial.surfaceType, hit.hitPoint, hit.normal);
        // }
    }
}