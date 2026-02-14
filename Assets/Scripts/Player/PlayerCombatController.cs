using UnityEngine;

[RequireComponent(typeof(PlayerInputState))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerCombatController : MonoBehaviour
{
    private PlayerInputState input;
    private PlayerStats stats;
    private WeaponController weaponController;

    void Awake()
    {
        input = GetComponent<PlayerInputState>();
        stats = GetComponent<PlayerStats>();
        weaponController = GetComponentInChildren<WeaponController>();
    }

    void Update()
    {
        if (stats.IsDead)
            return;

        HandleCombat();
    }

    private void HandleCombat()
    {
        if (input.IsAttacking)
            weaponController.TryAttack();
    }

    public void OnHit()
    {
        weaponController.PerformAttack();
    }
}
