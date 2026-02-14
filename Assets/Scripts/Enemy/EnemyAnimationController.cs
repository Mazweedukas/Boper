using PurrNet;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : NetworkBehaviour
{
    NetworkAnimator animator;
    int attackTriggerHash, spawnTriggerHash, dieTriggerHash, speedHash, staggerTriggerHash;

    protected override void OnSpawned()
    {
        base.OnSpawned();
        enabled = isServer;
    }

    void Awake()
    {
        animator = GetComponent<NetworkAnimator>();

        attackTriggerHash = Animator.StringToHash("Attack");
        dieTriggerHash = Animator.StringToHash("Die");
        spawnTriggerHash = Animator.StringToHash("Spawn");
        speedHash = Animator.StringToHash("Speed");
        staggerTriggerHash = Animator.StringToHash("Stagger");
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat(speedHash, speed);
    }

    public void PlayAttack()
    {
        animator.SetTrigger(attackTriggerHash);
    }

    public void PlayDeath()
    {
        animator.SetTrigger(dieTriggerHash);
    }

    public void Stagger()
    {
        animator.SetTrigger(staggerTriggerHash);
    }

    public void PlaySpawn()
    {
        animator.SetTrigger(spawnTriggerHash);
    }
}