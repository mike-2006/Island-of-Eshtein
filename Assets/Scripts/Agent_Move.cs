using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent_Move : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float updateInterval = 1f;

    private NavMeshAgent agent;
    private float _nextUpdateTime;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (Time.time >= _nextUpdateTime)
        {
            agent.SetDestination(player.position);
            _nextUpdateTime = Time.time + updateInterval;
        }

        
    }
}
