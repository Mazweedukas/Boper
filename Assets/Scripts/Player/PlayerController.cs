using PurrNet;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;

        if (!isOwner) return;

        CameraFollow cam = FindFirstObjectByType<CameraFollow>();
        cam.SetTarget(transform);
    }
}