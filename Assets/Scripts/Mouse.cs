using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] private int _aliveLayer;
    [SerializeField] private LayerMask _aliveAvoidMask;
    [SerializeField] private int _deadLayer;
    [SerializeField] private LayerMask _deadAvoidMask;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _deadMoveSpeed = 5f;
    [SerializeField] private float _chanceToChangeDirection = 0.3f;
    [SerializeField] private float _minTimeToChangeDirection = 2f;
    [SerializeField] private Transform _leftDownRayOrigin;
    [SerializeField] private Transform _rightDownRayOrigin;

    private bool _isAlive;
    private Animator _animator;
    private Rigidbody _rb;

    private int _direction = 1;
    private float _lastDirectionChangeTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _isAlive = true;
        SetAlive(_isAlive);
    }

    private void SetAlive(in bool isAlive)
    {
        _isAlive = isAlive;
        _animator.SetBool("IsAlive", isAlive);
        if (isAlive)
        {
            _rb.useGravity = true;
            _rb.isKinematic = false;

            gameObject.layer = _aliveLayer;
        }
        else
        {
            _rb.velocity = Vector3.zero;

            _rb.useGravity = false;
            _rb.isKinematic = false;

            gameObject.layer = _deadLayer;
        }
    }

    private void FixedUpdate()
    {
        var maxSpeed = _isAlive ? _moveSpeed : _deadMoveSpeed;
        maxSpeed *= _direction;

        var downRay = _direction > 0f ? _rightDownRayOrigin : _leftDownRayOrigin;
        Debug.DrawRay(downRay.position, Vector3.down, Color.green);

        if (_isAlive)
        {
            var hasStopAhead = Physics.Raycast(new Ray(downRay.position, Vector3.right * _direction), 0.2f, _aliveAvoidMask);
            var hasFloor = Physics.Raycast(new Ray(downRay.position, Vector3.down), 1f, _aliveAvoidMask);

            if (!hasFloor || hasStopAhead)
            {
                ChangeDirection();
                return;
            }

            if (Random.value <= _chanceToChangeDirection && (Time.time - _lastDirectionChangeTime) >= _minTimeToChangeDirection)
            {
                ChangeDirection();
                return;
            }

            var pos = transform.position + (transform.right * maxSpeed * Time.deltaTime);
            _rb.MovePosition(pos);
        }
        else
        {

        }
    }

    private void ChangeDirection()
    {
        _rb.velocity = Vector3.zero;
        _direction *= -1;
        _lastDirectionChangeTime = Time.time;
    }

    public void HitByCat()
    {
        SetAlive(false);
    }
}
