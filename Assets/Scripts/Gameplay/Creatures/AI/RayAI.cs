using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RayAI : MonoBehaviour
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
    private Transform target;

    public Transform fruitHeldPos;
    public float hungryInterval = 5f;
    public float hungryTimer;   // only after it count down to 0, the creature can detect and chase food
    public float eatDuration = 7f;
    public bool finishEating = true;

    void Start() {
        wanderCenter = transform.position;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject");
            enabled = false;
            return;
        }
        idleTimer = 0f;
        //eatTimer = 0f;
        hungryTimer = hungryInterval;
        SetNewWanderDestination();
    }

    void Update() {
        if (hungryTimer > 0f) {
            hungryTimer -= Time.deltaTime;
        }else if (hungryTimer < 0f){
            hungryTimer = 0f;
        }

        //if (eatTimer > 0f) {
        //    eatTimer -= Time.deltaTime;
        //} else if (eatTimer <= 0f && target != null && currentState != AnimalState.Chase) {
        //    Destroy(target.gameObject);
        //    hungryTimer = hungryInterval;
        //}
        switch (currentState) {
            case AnimalState.Wander:
                HandleWanderState();
                break;
            case AnimalState.Chase:
                HandleChaseState();
                break;
            //case AnimalState.Flee:
            //    HandleFleeState();
            //    break;
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
                finishEating = false;

                await UniTask.WaitForSeconds(2f);
                if (!isCanceled) {
                    // move fruit to antenna, restart hungry timer
                    if (shouldPrintDebug) print("food eaten, start idling");
                    target.GetComponent<NoiseFruit>().Eat();
                    target.SetParent(fruitHeldPos);
                    target.localPosition = Vector3.zero;
                    //eatTimer = eatDuration;

                    currentState = AnimalState.Idle;
                    currentIdleDuration = Random.Range(1f, 2f);
                    idleTimer = 0f;
                }else{
                    return;
                }
                

                await UniTask.WaitForSeconds(eatDuration);
                if (!isCanceled) {
                    Destroy(target.gameObject);
                    hungryTimer = hungryInterval;
                    finishEating = true;
                }else{
                    return;
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
        
    }

    void HandleIdleState() {
        agent.isStopped = true;
        idleTimer += Time.deltaTime;
        if (idleTimer >= currentIdleDuration) {
            DetectNextDest();
            idleTimer = 0f;
        }
    }

    void DetectNextDest() {
        if(hungryTimer == 0 && finishEating){
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
            Transform closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Collider hitCollider in hitColliders) {

                if (hitCollider.GetComponent<NoiseFruit>()) {
                    if (shouldPrintDebug) print("found food: " + hitCollider.gameObject.name);
                    float distanceSqr = (transform.position - hitCollider.transform.position).sqrMagnitude;
                    float vDistance = Mathf.Abs(transform.position.y - hitCollider.transform.position.y);
                    if (vDistance < 2f && distanceSqr < closestDistanceSqr) {
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
                return;
            }

        }

        if (shouldPrintDebug) print("food not found, start wandering");
        currentState = AnimalState.Wander;
        agent.isStopped = false;
        SetNewWanderDestination();
    }

    public bool IsHoldingFood(){
        return !finishEating && target != null;
    }

    public Transform GetHoldingFood(){
        return target;
    }

    private void OnDestroy() {
        isCanceled = true;
        Destroy(agent);
    }
}