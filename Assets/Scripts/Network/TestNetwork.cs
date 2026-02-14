using PurrNet;
using UnityEngine;

public class TestNetwork : NetworkIdentity
{
    [SerializeField] private NetworkIdentity _prefab;

    protected override void OnSpawned()
    {
        base.OnSpawned();
        if (!isServer)
            return;

        Instantiate(_prefab, Vector3.zero, Quaternion.identity);
    }
}
