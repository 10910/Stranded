using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiperAI : MonoBehaviour
{
    public AnimalState currentState = AnimalState.Wander;
    public float wanderRadius = 10f;
    public float detectionRadius = 12f;
    public float navmeshCheckRadius = 1f;
    public float wanderTimer = 3f;
    public float threatCheckRadius = 2f;
    public float fleeDistance = 15f;
    public int tryTimes = 10;
    public Vector3 wanderCenter;
    public float currentIdleDuration = 0f;
    public bool isCanceled = false;
    public bool shouldPrintDebug;

    private float idleTimer;
    private NavMeshAgent agent;
    private Transform target;
    private Scallop targetScallop;
    private Vector3 threatDir = Vector3.zero; 
    private float threatCheckTimer = 0f, threatCheckInterval = 1f;

    void Start() {
        wanderCenter = transform.position;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject");
            enabled = false;
            return;
        }
        idleTimer = 0f;
        threatCheckTimer = 0f;
        SetNewWanderDestination();
        currentState = AnimalState.Wander;
    }

    void Update() {
        threatCheckTimer += Time.deltaTime;
        if (threatCheckTimer > threatCheckInterval) {
            threatCheckTimer = 0f;
            CheckThreat();
        }

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

    void CheckThreat(){
        var colliders = Physics.OverlapSphere(transform.position, threatCheckRadius, LayerMask.GetMask("Player"));
        if (colliders.Length > 0) {
            //if (shouldPrintDebug) print("threatend");
            threatDir = colliders[0].transform.position - transform.position;
            // only on xz plane
            threatDir.y = 0;
            threatDir = threatDir.normalized;
        }else{
            threatDir = Vector3.zero;
        }
    }

    private void OnDrawGizmos() {
        if (agent != null && agent.enabled && agent.hasPath) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(agent.destination, 0.5f);
        }

        // wander detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wanderCenter, wanderRadius);

        // threat detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, threatCheckRadius);
    }

    void HandleWanderState() {
        if (threatDir != Vector3.zero) {
            SwitchToFleeState();
            return;
        }
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
                if (shouldPrintDebug) print("found new wander dest: " + hit.position);
                break;
            }
        }
    }

    async void HandleChaseState() {
        if (threatDir != Vector3.zero){
            SwitchToFleeState();
            return;
        }
        if (target != null && agent != null && agent.isOnNavMesh) {
            Vector3 dir = transform.position - target.position;
            Vector3 targetPos = target.position + dir.normalized * 2f; //body length
            agent.SetDestination(targetPos);
            if (!agent.pathPending && agent.remainingDistance < 0.01f) {
                if(shouldPrintDebug) print("food chased, start eating");
                currentState = AnimalState.Eat;
                agent.isStopped = true;

                await UniTask.WaitForSeconds(3f);
                //if (!isCanceled) {
                    targetScallop.Eaten();
                    targetScallop = null;
                    if (shouldPrintDebug) print("food eaten, start idling");
                    currentState = AnimalState.Idle;
                    currentIdleDuration = Random.Range(2f, 3f);
                    idleTimer = 0f;
                //}else{
                //    print("await canceled");
                //    isCanceled = true;
                //}
            }
        }
        else
        {
            if (shouldPrintDebug) print("target loss, start wandering");
            currentState = AnimalState.Wander;
        }
    }

    void HandleEatState(){
        if (threatDir != Vector3.zero) {
            SwitchToFleeState();
            return;
        }
    }

    void HandleFleeState() {
        // if the agent reaches the destination, switch to idle state
        if (!agent.pathPending && agent.remainingDistance < 0.01f) {
            currentState = AnimalState.Idle;
            agent.isStopped = true;
            currentIdleDuration = Random.Range(1f, 2f);
            idleTimer = 0f;
            wanderCenter = transform.position;
        }
    }

    void HandleIdleState() {
        if (threatDir != Vector3.zero) {
            SwitchToFleeState();
            return;
        }
        agent.isStopped = true;
        idleTimer += Time.deltaTime;
        if (idleTimer >= currentIdleDuration) {
            DetectFood();
            idleTimer = 0f;
        }
    }

    void SwitchToFleeState(){
        Vector3 fleeTarget = transform.position - threatDir * fleeDistance;
        NavMeshHit hit;
        bool found = false;
        for (int i = 0; i < tryTimes; i++) {
            if (NavMesh.SamplePosition(fleeTarget, out hit, 3f, NavMesh.AllAreas)) {
                agent.SetDestination(hit.position);
                if (shouldPrintDebug) print("start flee");
                found = true;
                break;
            }
        }
        if (!found) {
            if (shouldPrintDebug) print("flee target not found");
        }
        //else {
        //    // 如果找不到逃跑点，可以尝试随机移动
        //    SetNewWanderDestination();
        //}
        //isCanceled = true;
        currentState = AnimalState.Flee;
        agent.isStopped = false;
    }

    void DetectFood() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        // find nearest food
        foreach (Collider hitCollider in hitColliders) {
            var scallop = hitCollider.GetComponent<Scallop>();
            if (scallop != null && !scallop.isEaten) {
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
            targetScallop = closestTarget.GetComponent<Scallop>();
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