using System.Collections;
using PurrNet;
using UnityEngine;

public class EnemyHitStun : NetworkBehaviour
{
    public bool IsStunned { get; private set; }
    Coroutine stunRoutine;

    public void Apply(float duration)
    {
        if (duration <= 0f)
            return;

        if (stunRoutine != null)
            StopCoroutine(stunRoutine);

        stunRoutine = StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        IsStunned = true;

        yield return new WaitForSeconds(duration);

        IsStunned = false;
    }

    public void Stop()
    {
        if (stunRoutine != null)
            StopCoroutine(stunRoutine);

        IsStunned = false;
        stunRoutine = null;
    }
}