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
    private Vector3 _prevDirection;


    public Vector2 Input { get {  return new Vector2(_joystick.Horizontal, _joystick.Vertical); } }
    public Vector2 Velocity { get { return new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z); } }
    

    //#Run Value
    [SerializeField] private float _baseSpeed = 5.5f;
    [SerializeField] private float _acceleration = 2.5f;
    [SerializeField] private float _decceleration = 3.85f;

    private float _speedMultiplier = 1f;

    //#Condition
    private bool _inStunned;
    private bool _hasKnocked;

#endregion
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleRun();
    }

    protected virtual void HandleRun()
    {
        //Input data
        _horizontal = Vector3.right * _joystick.Horizontal;
        _vertical = Vector3.forward * _joystick.Vertical;
        Vector3 _direction = _horizontal + _vertical;

        // if no input, keep going the previous direction
        if(_direction == Vector3.zero)
        {
            _rigidbody.AddForce(_prevDirection * _baseSpeed * _speedMultiplier * 10f, ForceMode.Force);
            return;
        }

        _rigidbody.AddForce(_direction.normalized * _baseSpeed * _speedMultiplier * 10f, ForceMode.Force);
        _prevDirection = _direction;
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
}
