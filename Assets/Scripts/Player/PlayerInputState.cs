using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputState : NetworkBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsAiming { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsAimReady { get; private set; }
    public bool IsReloading { get; private set; }
    public bool Debug_1 { get; private set; }
    public bool Debug_2 { get; private set; }
    public bool Debug_3 { get; private set; }

    private InputSystem_Actions input;

    void Awake()
    {
        input = new InputSystem_Actions();
        BindInputs();
    }
    public void SetAimReady(bool value)
    {
        IsAimReady = value;
    }
    void OnEnable() => input.Player.Enable();
    void OnDisable() => input.Player.Disable();
    void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void BindInputs()
    {
        input.Player.Move.started += OnMove;
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;

        input.Player.Sprint.started += ctx => IsSprinting = true;
        input.Player.Sprint.canceled += ctx => IsSprinting = false;

        input.Player.Aim.started += ctx => IsAiming = true;
        input.Player.Aim.canceled += ctx => IsAiming = false;

        input.Player.Crouch.started += ctx => IsCrouching = true;
        input.Player.Crouch.canceled += ctx => IsCrouching = false;

        input.Player.Attack.started += ctx => IsAttacking = true;
        input.Player.Attack.canceled += ctx => IsAttacking = false;

        input.Player.Reload.started += ctx => IsReloading = true;
        input.Player.Reload.canceled += ctx => IsReloading = false;

        input.Player.Debug_1.started += ctx => Debug_1 = true;
        input.Player.Debug_1.canceled += ctx => Debug_1 = false;

        input.Player.Debug_2.started += ctx => Debug_2 = true;
        input.Player.Debug_2.canceled += ctx => Debug_2 = false;

        input.Player.Debug_3.started += ctx => Debug_3 = true;
        input.Player.Debug_3.canceled += ctx => Debug_3 = false;
    }
}