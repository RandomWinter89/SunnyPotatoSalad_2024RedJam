using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] CharacterGrowthDataSO _characterGrowth;

#region <<<Variable>>>
    [SerializeField] Joystick _joystick;

    //#Rigidbody
    private Rigidbody _rigidbody;

    //#Controller Value
    private Vector3 _horizontal;
    private Vector3 _vertical;
    private Vector3 _prevForce;


    public Vector2 Input { get {  return new Vector2(_joystick.Horizontal, _joystick.Vertical); } }
    public Vector2 Velocity { get { return new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z); } }
    

    //#Run Value
    [SerializeField] private float _baseSpeed = 5.5f;
    [SerializeField] private float _acceleration = 2.5f;
    [SerializeField] private float _decceleration = 3.85f;

    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float _knockbackForce = 1f;

    private float _speedMultiplier = 1f;
    private float _maneuvarability = 1f;

    //#Condition
    private bool _inStunned;
    private bool _hasKnocked;

    private float _stunDuration = 0;

#endregion
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_stunDuration > 0)
        {
            _stunDuration -= Time.deltaTime;
            return;
        }

        HandleRun();
    }

    protected virtual void HandleRun()
    {
        Vector2 input = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        Vector2 movement = input.normalized * _baseSpeed * _speedMultiplier * 10f;

        // if no input, keep going the previous direction
        if (input == Vector2.zero)
        {
            _rigidbody.AddForce(_prevForce);
            return;
        }
        Vector3 force = new Vector3(movement.x, 0f, movement.y);
        _rigidbody.AddForce(force * _maneuvarability);

        _prevForce = force;
    }

    public void GrowthKey(int _keyID)
    {
        _baseSpeed = _characterGrowth.growthStageDatas[_keyID].speedMultiplier;
        _rigidbody.drag = _characterGrowth.growthStageDatas[_keyID].manueverability;
    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        this._speedMultiplier = speedMultiplier;
    }

    public void SetManuevarability(float manuevarability)
    {
        manuevarability = Mathf.Clamp(manuevarability, .3f, 1f);
        this._maneuvarability = manuevarability;
    }

    private void SetStunDuration(float duration)
    {
        _stunDuration = duration;
    }

    [NaughtyAttributes.Button]
    public void Knockback()
    {
        Vector3 dir = -_prevForce.normalized;
        Vector3 force = dir * _knockbackForce;
        _rigidbody.AddForce( force, ForceMode.Impulse);
        SetStunDuration(stunDuration);
    }
}
