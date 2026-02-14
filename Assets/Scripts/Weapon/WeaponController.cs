using UnityEngine;

[RequireComponent(typeof(WeaponVFX))]
public class WeaponController : MonoBehaviour
{
    //This script is responsible for weapon attackSpeed, attacking logic.
    public float attackPerSecond = 1f;
    private float lastAttackTime;
    public enum AttackMode { manual, automatic };
    public AttackMode attackMode = AttackMode.manual;
    public int layerMask;
    private RangedAttack rangedAttack;
    private MeleeAttack meleeAttack;
    private WeaponVFX weaponVFX;
    private HitResult hitResult;
    private WeaponAmmo weaponAmmo;
    private PlayerAnimationController animationController;

    public bool CanAttack => Time.time >= lastAttackTime + 1 / attackPerSecond;

    void Awake()
    {
        layerMask = ~LayerMask.GetMask("Player");
        lastAttackTime = 0f;

        rangedAttack = GetComponent<RangedAttack>();
        if (rangedAttack != null)
            weaponAmmo = GetComponent<WeaponAmmo>();
        meleeAttack = GetComponent<MeleeAttack>();
        weaponVFX = GetComponent<WeaponVFX>();
        // weaponAudio = GetComponentInChildren<WeaponAudio>();

        animationController = GetComponentInParent<PlayerAnimationController>();
    }
    public void TryAttack()
    {
        if (!CanAttack)
            return;

        lastAttackTime = Time.time;

        animationController.TriggerMeleeAttackAnimation();
    }

    public void PerformAttack()
    {

        if (rangedAttack != null)
        {
            if (weaponAmmo.CanFire())
            {
                hitResult = rangedAttack.Attack(layerMask);
                weaponAmmo.ConsumeAmmo();
            }
            else
            {
                // weaponAudio.PlayEmptySound();
                return;
            }
        }

        if (meleeAttack != null)
        {
            meleeAttack.Attack(layerMask);

        }

        weaponVFX.PlayShot(hitResult);
        weaponVFX.PlayHitEffect(hitResult);
        // weaponAudio.PlayAttackSound();
        // weaponAudio.PlayHitSound(hitResult);
    }
}