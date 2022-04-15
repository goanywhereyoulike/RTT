using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int index = -1;
    [SerializeField] private Rigidbody enemyRb;
    [SerializeField] private Collider enemyCollider;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private EnemyNav enemyNav;
    [SerializeField] private CloseDetect closeDetect;
    [SerializeField] private FieldOfView fov;
    [SerializeField] private Entity enemyEntity;
    [SerializeField] private EnemyState state = EnemyState.Alive;
    public float hitTime;
    public float invlunTime = 1.0f;

    enum EnemyState
    {
        Alive,
        Alret,
        Dead
    }

    private void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody>();
        enemyCollider = gameObject.GetComponent<Collider>();
        //enemyAnim = gameObject.GetComponent<Animator>();
        pathFollower = gameObject.GetComponent<PathFollower>();
        enemyNav = gameObject.GetComponent<EnemyNav>();
        closeDetect = gameObject.GetComponent<CloseDetect>();
        fov = gameObject.GetComponent<FieldOfView>();
        enemyEntity = GetComponent<Entity>();
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Alive:
                if (enemyEntity.IsDead())
                {
                    state = EnemyState.Dead;
                }
                break;
            case EnemyState.Alret:
                if (enemyEntity.IsDead())
                {
                    state = EnemyState.Dead;
                }
                break;
            case EnemyState.Dead:
                DisableEnemyFunction();
                //enemyAnim.SetBool("IsDead", true);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage") && Time.time > hitTime + invlunTime)
        {
            Debug.Log("HIt");
            Damage damageIncoming = other.gameObject.GetComponent<Damage>();
            enemyEntity.TakeDamage(damageIncoming.damageAmount);
            hitTime = Time.time;
        }
    }

    void DisableEnemyFunction()
    {
        enemyCollider.enabled = false;
        //enemyAnim.enabled = false;
        pathFollower.enabled = false;
        enemyNav.enabled = false;
        closeDetect.enabled = false;
        fov.enabled = false;
    }

}
