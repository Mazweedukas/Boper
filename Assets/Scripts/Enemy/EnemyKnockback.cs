using System.Collections;
using PurrNet;
using UnityEngine;

public class EnemyKnockback : NetworkBehaviour
{
    Coroutine knockRoutine;

    // protected override void OnSpawned()
    // {
    //     base.OnSpawned();
    //     enabled = isServer;
    // }

    public void Apply(Vector3 direction, float force, CharacterController characterController, float duration = 0.1f)
    {
        direction.y = 0f;
        direction.Normalize();

        if (knockRoutine != null)
            StopCoroutine(knockRoutine);

        knockRoutine = StartCoroutine(KnockbackRoutine(direction, force, duration, characterController));
    }


    IEnumerator KnockbackRoutine(Vector3 dir, float force, float duration, CharacterController characterController)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float step = force / duration * Time.deltaTime;
            characterController.Move(dir * step);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void Stop()
    {
        if (knockRoutine != null)
            StopCoroutine(knockRoutine);

        knockRoutine = null;
    }
}