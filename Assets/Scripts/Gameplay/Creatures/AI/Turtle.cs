using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Wander,
    Chase,
    Eat,
    Flee,
    Idle
}
public class AnimalAI : MonoBehaviour
{


    public AnimalState currentState = AnimalState.Wander;
    public float wanderRadius = 10f;
    public float detectionRadius = 12f;
    public float navmeshCheckRadius = 1f;
    public float wanderTimer = 3f;
    public int tryTimes = 10;
    public Vector3 wanderCenter;
    public float currentIdleDuration = 0f;
    public bool isCanceled = false;
    public bool shouldPrintDebug;

    private float idleTimer;
    private NavMeshAgent agent;
    private Transform target; // ��ѡ��׷��Ŀ��
    private Transform threat; // ��ѡ��������в


    void Start() {
        wanderCenter = transform.position;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject");
            enabled = false;
            return;
        }
        idleTimer = 0f;
        SetNewWanderDestination();
    }

    void Update() {
        switch (currentState) {
            case AnimalState.Wander:
                HandleWanderState();
                break;
            case AnimalState.Chase:
                HandleChaseState();
                break;
            case AnimalState.Flee:
                HandleFleeState();
                break;
            case AnimalState.Idle:
                HandleIdleState();
                break;
            case AnimalState.Eat:
                HandleEatState();
                break;
        }
    }

    private void OnDrawGizmos() {
        if (agent != null && agent.enabled && agent.hasPath) {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(agent.destination, 0.5f); // 0.5f ������İ뾶������Ե�����
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wanderCenter, wanderRadius);
    }

    void HandleWanderState() {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            currentState = AnimalState.Idle;
            agent.isStopped = true;
            currentIdleDuration = Random.Range(1f, 2f);
            idleTimer = 0f;
        }
    }

    void SetNewWanderDestination() {
        for (int i = 0; i < tryTimes; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            Vector3 wanderDest = wanderCenter + randomDirection;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(wanderDest, out hit, navmeshCheckRadius, NavMesh.AllAreas)) {
                agent.SetDestination(hit.position);
            }
        }
    }

    async void HandleChaseState() {
        
        if (target != null && agent != null && agent.isOnNavMesh) {
            Vector3 dir = transform.position - target.position;
            Vector3 targetPos = target.position + dir.normalized * 3f; //body length
            agent.SetDestination(targetPos);
            if (!agent.pathPending && agent.remainingDistance < 0.01f) {
                if(shouldPrintDebug) print("food chased, start eating");
                currentState = AnimalState.Eat;
                agent.isStopped = true;

                await UniTask.WaitForSeconds(5f);
                if (!isCanceled) {
                    Destroy(target.gameObject);
                    if (shouldPrintDebug) print("food eaten, start idling");
                    currentState = AnimalState.Idle;
                    currentIdleDuration = Random.Range(2f, 3f);
                    idleTimer = 0f;
                }
            }
        }
        else
        {
            if (shouldPrintDebug) print("target loss, start wandering");
            currentState = AnimalState.Wander;
        }
    }

    void HandleEatState(){

    }

    void HandleFleeState() {
        if (threat != null && agent != null && agent.isOnNavMesh) {
            Vector3 fleeDirection = (transform.position - threat.position).normalized * wanderRadius; // ʹ�� wanderRadius ��Ϊ���ܾ���
            Vector3 fleeTarget = transform.position + fleeDirection;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, wanderRadius, NavMesh.AllAreas)) {
                agent.SetDestination(hit.position);
            }
            else {
                // ����Ҳ������ܵ㣬���Գ�������ƶ�
                SetNewWanderDestination();
            }
        }
        else {
            // ��в��ʧ���޷��������л����ε�
            currentState = AnimalState.Wander;
        }
    }

    void HandleIdleState() {
        agent.isStopped = true;
        idleTimer += Time.deltaTime;
        if (idleTimer >= currentIdleDuration) {
            DetectFood();
            idleTimer = 0f;
        }
    }

    void DetectFood() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.GetComponent<TurtleFruit>() && hitCollider.gameObject.layer == LayerMask.NameToLayer("Environment")) {
                if (shouldPrintDebug) print("found food: " + hitCollider.gameObject.name);
                float distanceSqr = (transform.position - hitCollider.transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr) {
                    closestDistanceSqr = distanceSqr;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        if (closestTarget != null) {
            if (shouldPrintDebug) print("food found, start chasing");
            target = closestTarget;
            currentState = AnimalState.Chase;
            agent.isStopped = false;
        }
        else {
            if (shouldPrintDebug) print("food not found, start wandering");
            currentState = AnimalState.Wander;
            agent.isStopped = false;
            SetNewWanderDestination();
        }
    }
}