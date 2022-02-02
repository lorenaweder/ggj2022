using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cat : MonoBehaviour
{
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
    [Space]
    [SerializeField] private float _invincibleTime = 1f;
    [SerializeField] private float _invincibleTimeChange = 0.2f;

    [Header("Functionality")]
    [SerializeField] private int _deadLayer;
    [SerializeField] private int _aliveLayer;
    [SerializeField] private int _insideShitLayer;
    [SerializeField] private LayerMask _insideCheckLayerMask;
    [Space]
    [SerializeField] private SpriteRenderer _renderer;

    private bool _isAlive = true;
    private bool _isInsideShit = false;

    private float _horizontal;
    private float _vertical;

    private float _impulse = 0f;
    private float _invincibleTimer = 0f;
    private bool _hitByMice;

    private Animator _animator;
    private SphereCollider _collider;
    private Rigidbody _rb;
    private Collider[] _stuffNear = new Collider[5];

    private Lives _lives;

    public bool IsAlive => _isAlive;

    private void Awake()
    {
        MessageDispatcher.Score = 0;

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();

        _lives = GetComponent<Lives>();

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

        // Handle sprite facing
        if (Mathf.Abs(_horizontal) > 0.01f)
        {
            _renderer.flipX = _horizontal < 0f;
        }

        // Handle invincible
        if (_invincibleTimer >= 0f)
        {
            _invincibleTimer -= Time.deltaTime;
            if(_hitByMice) _renderer.enabled = Time.frameCount % 5 == 0;
        }
        else
        {
            _hitByMice = false;
            _renderer.enabled = true;
        }
    }

    private void SetAlive(in bool isAlive)
    {
        _animator.SetBool("IsAlive", isAlive);

        _rb.useGravity = isAlive;
        _rb.isKinematic = false;

        if (!isAlive)
        {
            // Keep a longer timer if just hit
            _invincibleTimer = Mathf.Max(_invincibleTimer, _invincibleTimeChange);

            SoundManager.Instance.PlayEffectMusic("MusicCatDeath", true);
            _isInsideShit = false;
            _impulse = _initialDeadImpulse;

            SetLayerOnSelfAndChildren(_deadLayer);
        }
        else
        {
            _rb.velocity = Vector3.zero;
            SoundManager.Instance.PlayEffectMusic("MusicCatAlive", true);

            var hits = Physics.OverlapSphereNonAlloc(transform.position + _collider.center, _collider.radius * 0.8f,
                _stuffNear, _insideCheckLayerMask);

            if (hits > 0)
            {
                _isInsideShit = true;
                SetLayerOnSelfAndChildren(_insideShitLayer);
            }
            else
            {
                _isInsideShit = false;
                SetLayerOnSelfAndChildren(_aliveLayer);
            }
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

        if (_isInsideShit)
        {
            var hits = Physics.OverlapSphereNonAlloc(transform.position + _collider.center, _collider.radius,
                _stuffNear, _insideCheckLayerMask);

            if (hits <= 0)
            {
                _isInsideShit = false;
                SetLayerOnSelfAndChildren(_aliveLayer);
            }
        }
        
        if (!_isAlive)
        {
            _rb.AddForce(Vector3.up * _impulse, ForceMode.Force);
            _impulse = Mathf.Max(_minDeadUpMovement, _impulse - _impulseDecay * Time.deltaTime);
        }
    }

    private void SetLayerOnSelfAndChildren(int layer)
    {
        gameObject.layer = layer;
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = layer;
        }
    }

    // Live/Dead interactions guaranteed by the layer setup
    // dead cat only hits dead mice; live cat/live mice
    public void HitMouse()
    {
        if (_isAlive)
        {
            MessageDispatcher.Score++;
            return;
        }
        if (_invincibleTimer > 0f) return;
        if (!_lives.LoseOne())
        {
            SoundManager.Instance.PlayEffect("CatDamaged");
            _invincibleTimer = _invincibleTime;
            _hitByMice = true;
            return;
        } else {
            SoundManager.Instance.PlayEffect("CatDied");
        }
        MessageDispatcher.NotifyGameOver();
        this.enabled = false;
    }
}