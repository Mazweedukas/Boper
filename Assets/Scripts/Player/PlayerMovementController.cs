using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerInputState))]
public class PlayerMovementController : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerInputState playerInputState;
    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        stats = GetComponent<PlayerStats>();
        playerInputState = GetComponent<PlayerInputState>();
    }

    void Update()
    {
        if (stats.IsDead)
            return;

        Move();
    }

    private void Move()
    {
        Vector3 worldMove = new Vector3(playerInputState.MoveInput.x, 0f, playerInputState.MoveInput.y);

        float speed = stats.movementSpeed;

        if (worldMove.sqrMagnitude > 1f)
            worldMove.Normalize();

        if (playerInputState.IsSprinting)
            speed *= stats.sprintMultiplier;

        characterController.Move(speed * Time.deltaTime * worldMove);
    }
}
