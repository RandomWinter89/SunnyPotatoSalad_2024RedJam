using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CharacterMovement_AI : MonoBehaviour
{
    [SerializeField] CharacterGrowthDataSO _characterGrowth;
    [SerializeField] private NavMeshAgent agent;
    

    public Vector2 Velocity { get { return new Vector2(agent.velocity.x, agent.velocity.z); } }

//    [SerializeField, NaughtyAttributes.ReadOnly] private Vector3 destination;

    #region <<<Variable>>>

    //#Rigidbody
    private Rigidbody _rigidbody;

    //#Run Value
    [SerializeField] private float _baseSpeed = 5.5f;
    [SerializeField] private float _acceleration = 2.5f;
    [SerializeField] private float _decceleration = 3.85f;

    private float _speedMultiplier = 1f;

    //#Condition
    private bool _inStunned;
    private bool _hasKnocked;

    private float _remainingIdleTime = 2f;
    private Vector3 idleTimeMinMax = new Vector2(2f, 4f);
    

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetDestination(GetRandomAvailableDestination());
    }

    private void FixedUpdate()
    {
        HandleRun();
    }

    protected virtual void HandleRun()
    {
        if (_remainingIdleTime > 0f)
        {
            _remainingIdleTime -= Time.deltaTime;
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        if (HasArrived())
        {
            _remainingIdleTime = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
          
            SetDestination(GetRandomAvailableDestination());
        }
    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        this._speedMultiplier = speedMultiplier;
    }

    private Vector3 GetRandomAvailableDestination()
    {
        return WorldData.Instance.GetRandomWorldPosition();
    }

    private void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private bool HasArrived()
    {
        if (!agent.hasPath) return true;
        return false;
    }

    public void Stop()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        enabled = false;

    }
}