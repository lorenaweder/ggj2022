using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cat : MonoBehaviour
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
    }

    private void Start()
    {
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

        var layer = isAlive ? _aliveLayer : _deadLayer;
        gameObject.layer = layer;
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = layer;
        }

        _rb.useGravity = isAlive;
        _rb.isKinematic = false;

        if (!isAlive)
        {
            _impulse = _initialDeadImpulse;
        }

        MessageDispatcher.NotifyAlive(isAlive);
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

    public void HitMouse()
    {
        
    }
}