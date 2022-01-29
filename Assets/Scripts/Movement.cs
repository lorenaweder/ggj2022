using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private int _deadLayer;
    [SerializeField] private int _aliveLayer;

    [Header("Controls")]
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private KeyCode _toggleAlive = KeyCode.Space;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _deadMoveSpeed = 5f;
    [Space]
    [SerializeField] private float _initialDeadImpulse = 4f;
    [SerializeField] private float _minDeadUpMovement = 0.25f;
    [SerializeField] private float _impulseDecay = 5f;

    private float _impulse = 0f;

    private bool _isAlive = true;
    private float _horizontal;
    private float _vertical;

    private Animator _animator;
    private Rigidbody _rb;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _isAlive = Random.value >= 0.5f;
        SetAlive(_isAlive);
    }

    private void Update()
    {
        _horizontal = Input.GetAxis(_horizontalAxis);

        if (Input.GetKeyDown(_toggleAlive))
        {
            _isAlive = !_isAlive;
            SetAlive(_isAlive);
        }

        _vertical = _isAlive ? 0f : Input.GetAxis(_verticalAxis);
    }

    private void SetAlive(in bool isAlive)
    {
        _animator.SetBool("IsAlive", isAlive);
        if (isAlive)
        {
            _rb.useGravity = true;
            _rb.isKinematic = false;

            gameObject.layer = _aliveLayer;
        }
        else
        {
            _rb.useGravity = false;
            _rb.isKinematic = false;

            _impulse = _initialDeadImpulse;

            gameObject.layer = _deadLayer;
        }
    }

    private void FixedUpdate()
    {
        var s = _isAlive ? _moveSpeed : _deadMoveSpeed;
        Vector2 pos = transform.position
                      + (transform.right * s * Time.deltaTime * _horizontal)
                      + (transform.up * s * Time.deltaTime * _vertical);


        _rb.MovePosition(pos);
        
        if (!_isAlive)
        {
            _rb.AddForce(Vector3.up * _impulse, ForceMode.Force);
            _impulse = Mathf.Max(_minDeadUpMovement, _impulse - _impulseDecay * Time.deltaTime);
        }
    }
}