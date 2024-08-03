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


    public Vector2 Input { get {  return new Vector2(_joystick.Horizontal, _joystick.Vertical); } }

    //#Run Value
    [SerializeField] private float _maximumRunSpeed = 5.5f;
    [SerializeField] private float _acceleration = 2.5f;
    [SerializeField] private float _decceleration = 3.85f;

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

    private void HandleRun()
    {
        //Input data
        _horizontal = Vector3.right * _joystick.Horizontal;
        _vertical = Vector3.forward * _joystick.Vertical;
        Vector3 _direction = _horizontal + _vertical;

        _rigidbody.AddForce(_direction.normalized * _maximumRunSpeed * 10f, ForceMode.Force);
    }

    public void GrowthKey(int _keyID)
    {
        _maximumRunSpeed = _characterGrowth.growthStageDatas[_keyID].speed;
        _rigidbody.drag = _characterGrowth.growthStageDatas[_keyID].manueverability;
    }
}
