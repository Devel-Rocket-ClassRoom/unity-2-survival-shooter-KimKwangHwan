using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Zombie : LivingEntity
{
    public enum State
    {
        Idle,
        Trace,
        Attack,
        Die
    }
    private State _currentState;
    public State CurrentState
    {
        get { return _currentState; }
        set
        {
            var prevState = _currentState;
            _currentState = value;

            switch (_currentState)
            {
                case State.Idle:
                    animator.SetBool("TargetFound", false);
                    agent.isStopped = true;
                    break;
                case State.Trace:
                    animator.SetBool("TargetFound", true);
                    agent.isStopped = false;
                    break;
                case State.Attack:
                    animator.SetBool("TargetFound", false);
                    agent.isStopped = false;
                    break;
                case State.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    agent.enabled = false;
                    hitBox.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public HitBox hitBox;
    public Transform target;
    public LayerMask targetLayer;
    public ParticleSystem hitEffect;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audio;

    private bool isSinking = false;
    private float damage;
    private float speed;
    private float attackInterval;
    private int score;

    private float traceDistance = 100f;
    private float lastAttackTime = 0;

    private AudioClip hurtClip;
    private AudioClip deathClip;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        CurrentState = State.Idle;
        hitBox.gameObject.SetActive(true);
    }

    public void Setup(ZombieData data)
    {
        startingHealth = data.startingHealth;
        Health = startingHealth;
        damage = data.damage;
        speed = data.moveSpeed;
        agent.speed = speed;
        attackInterval = data.attackInterval;
        score = data.score;

        hurtClip = data.hurtClip;
        deathClip = data.deathClip;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Trace:
                UpdateTrace();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                UpdateDie();
                break;
        }
    }
    private void UpdateIdle()
    {
        if (target != null)
        {
            CurrentState = State.Trace;
        }
        target = FindTarget(traceDistance);
    }
    private void UpdateTrace()
    {
        if (target == null)
        {
            CurrentState = State.Idle;
        }

        if (target != null && hitBox.target != null)
        {
            CurrentState = State.Attack;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateAttack()
    {
        if (hitBox.target == null || target == null)
        {
            CurrentState = State.Trace;
        }
        if (hitBox.target != null)
        {
            var lookAt = hitBox.target.position;
            lookAt.y = 0f;
            transform.LookAt(lookAt);

            if (Time.time > lastAttackTime + attackInterval)
            {
                lastAttackTime = Time.time;
                var livingEntity = hitBox.target.GetComponent<LivingEntity>();
                if (livingEntity != null && !livingEntity.IsDead)
                {
                    livingEntity.OnDamage(damage, hitBox.target.position, -hitBox.target.forward);
                }
            }
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead)
        {
            base.OnDamage(damage, hitPoint, hitNormal);
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.forward = hitNormal;
            hitEffect.Play();
            audio.PlayOneShot(hurtClip);
        }
    }

    public override void Die()
    {
        if (IsDead)
            return;
        base.Die();
        CurrentState = State.Die;
        audio.PlayOneShot(deathClip);
        GameManager.instance.AddScore(score);
    }

    private void UpdateDie()
    {
        
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        if (colliders.Length == 0)
            return null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.transform;
                break;
            }
        }
        return target.transform;
    }
    
    public void StartSinking()
    {
        isSinking = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
}
