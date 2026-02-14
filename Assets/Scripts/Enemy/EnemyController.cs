using PurrNet;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour, ITick
{
    private EnemyAnimationController enemyAnimationController;
    private EnemyCombat enemyCombat;
    private EnemyState currentState;
    private EnemyStats enemyStats;
    private EnemyRangeDetector enemyRangeDetector;
    public EnemyLifeCycleState LifeCycleState { get; set; }
    private NavMeshAgent agent;
    private GameObject[] detectedTargets;

    void Awake()
    {
        enemyAnimationController = GetComponent<EnemyAnimationController>();
        enemyCombat = GetComponent<EnemyCombat>();
        agent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRangeDetector = GetComponent<EnemyRangeDetector>();
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isServer;
        EnterSpawn();
    }

    void Update()
    {
        if (!isServer) return;
        if (LifeCycleState != EnemyLifeCycleState.Alive) return;

        enemyAnimationController.SetSpeed(agent.velocity.magnitude);

        currentState = EnemyStateUpdate();

        if (currentState == EnemyState.Destroy_Crystal)
            ChaseAndAttack("Crystal");
        else if (currentState == EnemyState.Destroy_Player)
            ChaseAndAttack("Player");
    }

    private EnemyState EnemyStateUpdate()
    {
        if (detectedTargets != null && detectedTargets.Length > 0)
            return EnemyState.Destroy_Player;
        else
            return EnemyState.Destroy_Crystal;
    }

    public void OnTick(float delta)
    {
        if (!isServer) return;

        detectedTargets = enemyRangeDetector.UpdateDetector(enemyStats.detectionRange);
    }

    public void EnterSpawn()
    {
        LifeCycleState = EnemyLifeCycleState.Spawning;

        agent.enabled = false;
        enemyCombat.enabled = false;
        enemyAnimationController.enabled = false;
        enemyRangeDetector.enabled = false;
        enemyStats.enabled = false;

        enemyAnimationController.PlaySpawn();
    }

    public void EnterAlive()
    {
        LifeCycleState = EnemyLifeCycleState.Alive;

        agent.enabled = true;
        enemyCombat.enabled = true;
        enemyAnimationController.enabled = true;
        enemyRangeDetector.enabled = true;
        enemyStats.enabled = true;
    }

    public void EnterDead()
    {
        LifeCycleState = EnemyLifeCycleState.Dead;

        agent.enabled = false;
        enemyCombat.enabled = false;
        enemyAnimationController.enabled = false;
        enemyRangeDetector.enabled = false;
        enemyStats.enabled = false;

        enemyAnimationController.PlayDeath();

        Destroy(gameObject, 3f);
    }

    public void ChaseAndAttack(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        if (targets.Length == 0) return;

        GameObject closest = targets[0];
        float minDist = Vector3.Distance(transform.position, closest.transform.position);

        foreach (GameObject t in targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        agent.SetDestination(closest.transform.position);
        if (HasReachedDestination())
        {
            RotateTowards(closest.transform.position);
            enemyCombat.TryAttack();
        }
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude != 0f)
            return false;

        return true;
    }

    private void RotateTowards(Vector3 target, float rotationSpeed = 5f)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
