using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public Transform target;
    public float maxSpeed = 4.0f;
    public NavMeshAgent _agent;
    public bool ChasePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        if (ChasePlayer)
        {
            _agent.destination = target.position;
            _agent.speed = 4.0f;
        }

    }
}
