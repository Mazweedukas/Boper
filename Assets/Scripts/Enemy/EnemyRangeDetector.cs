using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyRangeDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask detectionMask;
    private GameObject[] DetectedTargets { get; set; }

    public GameObject[] UpdateDetector(float detectionRange = 4f)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

        if (colliders.Length > 0)
        {
            DetectedTargets = new GameObject[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                DetectedTargets[i] = colliders[i].gameObject;
            }
        }
        else
        {
            DetectedTargets = null;
        }

        return DetectedTargets;
    }

    // Debug visualization
    // private void OnDrawGizmos()
    // {
    //     if (!showDebugVisuals || this.enabled == false) return;

    //     Gizmos.color = DetectedTargets != null ? Color.green : Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, enemyStats.detectionRange);

    // }
}
