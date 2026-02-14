using System;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDamageable
{

    public float health = 100f;

    public void TakeDamage(DamageInfo info)
    {
        health -= info.damage;
        if (health <= 0f)
        {
            //GG
        }
    }
}
