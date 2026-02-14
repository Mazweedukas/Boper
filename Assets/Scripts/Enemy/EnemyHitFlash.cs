using System.Collections;
using PurrNet;
using UnityEngine;

public class EnemyHitFlash : NetworkBehaviour
{
    public Color emissionColor = Color.white;
    public float intensity = 3f;
    public float duration = 0.08f;

    SkinnedMeshRenderer[] renderers;
    MaterialPropertyBlock block;
    Color[] originalEmission;

    static readonly int EmissionID = Shader.PropertyToID("_EmissionColor");

    // protected override void OnSpawned()
    // {
    //     base.OnSpawned();
    //     enabled = isServer;
    // }

    void Awake()
    {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        block = new MaterialPropertyBlock();

        originalEmission = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];
            if (r.sharedMaterial.HasProperty(EmissionID))
            {
                originalEmission[i] = r.sharedMaterial.GetColor(EmissionID);
            }
        }
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        Debug.Log("Flashing hit effect");
        Color flash = emissionColor * intensity;
        Debug.Log($"Flash color: {flash}");

        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];

            r.GetPropertyBlock(block);
            block.SetColor(EmissionID, flash);
            r.SetPropertyBlock(block);
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];

            r.GetPropertyBlock(block);
            block.SetColor(EmissionID, originalEmission[i]);
            r.SetPropertyBlock(block);
        }
    }
}