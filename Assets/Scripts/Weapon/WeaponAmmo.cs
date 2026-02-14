using System.Collections;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int magazineSize = 30;
    public int ammoInMagazine;
    public float reloadTime = 2f;

    public bool IsReloading { get; private set; }
    // private WeaponAudio weaponAudio;

    void Awake()
    {
        ammoInMagazine = magazineSize;
        // weaponAudio = GetComponent<WeaponAudio>();
    }

    public bool CanFire()
    {
        return ammoInMagazine > 0 && !IsReloading;
    }

    public void ConsumeAmmo()
    {
        ammoInMagazine--;
    }

    public void Reload(MonoBehaviour runner)
    {
        if (IsReloading || ammoInMagazine == magazineSize)
            return;

        runner.StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        // weaponAudio.PlayReloadSound();
        IsReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammoInMagazine = magazineSize;
        IsReloading = false;
    }
}