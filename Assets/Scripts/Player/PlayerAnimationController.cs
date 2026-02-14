using UnityEngine;
using PurrNet;
using JetBrains.Annotations;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputState))]
public class PlayerAnimationController : NetworkBehaviour
{
    private NetworkAnimator animator;
    private int speedHash, attackPrimaryHash, isSprintingHash, IsReloadingHash;
    private PlayerStats stats;
    private PlayerInputState playerInputState;
    private WeaponAmmo weaponAmmo;
    private CharacterController controller;
    public bool isSprinting;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<NetworkAnimator>();
        weaponAmmo = GetComponentInChildren<WeaponAmmo>();
        playerInputState = GetComponent<PlayerInputState>();
        controller = GetComponent<CharacterController>();

        attackPrimaryHash = Animator.StringToHash("Attack_Primary");
        speedHash = Animator.StringToHash("Speed");
        isSprintingHash = Animator.StringToHash("isSprinting");
        IsReloadingHash = Animator.StringToHash("isReloading");
    }

    //Solved bug with root movement
    void OnAnimatorMove() { }

    void Update()
    {
        isSprinting = playerInputState.IsSprinting;

        if (!isOwner)
            return;

        HandleAnimation();
    }

    private void HandleAnimation()
    {
        if (stats.IsDead)
        {
            return;
        }

        animator.SetBool(isSprintingHash, isSprinting);
        animator.SetBool(IsReloadingHash, playerInputState.IsReloading);

        SetSpeed(stats.movementSpeed);

        if (playerInputState.IsReloading)
            weaponAmmo.Reload(this);
    }
    public void TriggerMeleeAttackAnimation()
    {
        animator.SetTrigger(attackPrimaryHash);
    }

    public void SetSpeed(float maxSpeed = 3f)
    {
        float currentSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
        animator.SetFloat(speedHash, currentSpeed / maxSpeed);
    }
}