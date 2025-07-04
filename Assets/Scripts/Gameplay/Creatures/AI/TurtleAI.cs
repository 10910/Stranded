using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    Idle,
    Paraylsed,
    Startled
}
public class TurtleAI : MonoBehaviour
{
    public AnimalState currentState = AnimalState.Wander;
    public float wanderRadius = 10f;
    public float detectionRadius = 12f;
    public float navmeshCheckRadius = 1f;
    public float wanderTimer = 3f;
    public float startledTimer;
    public float startledDuration = 5f;
    public int tryTimes = 10;
    public Vector3 wanderCenter;
    public float currentIdleDuration = 0f;
    public bool isCanceled = false;
    public bool shouldPrintDebug;

    private float idleTimer;
    private NavMeshAgent agent;
    private Transform target;

    // retreat animation
    public float height = 2.2f;
    public float retreatDuration = 1f;

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
            case AnimalState.Idle:
                HandleIdleState();
                break;
            case AnimalState.Eat:
                HandleEatState();
                break;
            case AnimalState.Startled:
                HandleStartledState();
                break;
        }
    }

    private void OnDrawGizmos() {
        if (agent != null && agent.enabled && agent.hasPath) {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(agent.destination, 0.5f); // 0.5f 是球体的半径，你可以调整它
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

    //void HandleFleeState() {
    //    if (threat != null && agent != null && agent.isOnNavMesh) {
    //        Vector3 fleeDirection = (transform.position - threat.position).normalized * wanderRadius; // 使用 wanderRadius 作为逃跑距离
    //        Vector3 fleeTarget = transform.position + fleeDirection;
    //        NavMeshHit hit;
    //        if (NavMesh.SamplePosition(fleeTarget, out hit, wanderRadius, NavMesh.AllAreas)) {
    //            agent.SetDestination(hit.position);
    //        }
    //        else {
    //            // 如果找不到逃跑点，可以尝试随机移动
    //            SetNewWanderDestination();
    //        }
    //    }
    //    else {
    //        // 威胁丢失或无法导航，切换回游荡
    //        currentState = AnimalState.Wander;
    //    }
    //}

    void HandleIdleState() {
        agent.isStopped = true;
        idleTimer += Time.deltaTime;
        if (idleTimer >= currentIdleDuration) {
            DetectFood();
            idleTimer = 0f;
        }
    }

    void HandleStartledState() {
        startledTimer += Time.deltaTime;
        if(startledTimer >= startledDuration){
            if (shouldPrintDebug) print("turtle recover from startled");
            currentState = AnimalState.Idle;
            currentIdleDuration = 1f;
            startledTimer = 0;
            isCanceled = false;
            float dest = transform.position.y + height;
            print(dest);
            transform.DOMoveY(transform.position.y + height+ 0.3f, retreatDuration).SetEase(Ease.Linear).OnComplete(()=>{
                agent.updatePosition = true;
                idleTimer = 0f;
            });
        }
    }

    public void SwitchStartled(){
        if (currentState == AnimalState.Startled) {
            startledTimer = 0;
        }else{
            if (shouldPrintDebug) print("turtle startled");
            agent.updatePosition = false;
            agent.isStopped = true;
            isCanceled = true;
            transform.DOMoveY(transform.position.y - height, retreatDuration).SetEase(Ease.OutQuad);
            currentState = AnimalState.Startled;
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