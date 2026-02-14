using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputState))]
public class PlayerRotationController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private RotationMode rotationMode = RotationMode.LookAtMouse;

    private PlayerInputState inputState;
    void Awake()
    {
        inputState = GetComponent<PlayerInputState>();
    }
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = rotationMode switch
        {
            RotationMode.LookAtMouse => GetMouseDirection(),
            RotationMode.MoveDirection => GetMoveDirection(),
            _ => Vector3.zero
        };

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private Vector3 GetMouseDirection()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0f;
            return dir;
        }

        return Vector3.zero;
    }

    private Vector3 GetMoveDirection()
    {
        Vector2 moveInput = inputState.MoveInput;
        Vector3 dir = new Vector3(moveInput.x, 0f, moveInput.y);

        return dir.sqrMagnitude > 0.001f ? dir : Vector3.zero;
    }
}
