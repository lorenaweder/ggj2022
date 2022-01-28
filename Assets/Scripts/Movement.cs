using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private int _deadLayer;
    [SerializeField] private int _aliveLayer;
    [SerializeField] private float _moveSpeed = 5f;

    private bool _isAlive = true;
    private float _horizontal;
    private float _vertical;

    private Animator _animator;
    private Rigidbody _rb;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        SetAlive(_isAlive);
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isAlive = !_isAlive;
            SetAlive(_isAlive);
        }

        _vertical = _isAlive ? 0f : Input.GetAxis("Vertical");
    }

    private void SetAlive(in bool isAlive)
    {
        _animator.SetBool("IsAlive", isAlive);
        if (isAlive)
        {
            _rb.useGravity = true;
            _rb.isKinematic = false;
            gameObject.layer = _aliveLayer;
            CancelInvoke("SetGhost");
        }
        else
        {
            _rb.useGravity = false;
            _rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            _rb.isKinematic = false;
            gameObject.layer = _deadLayer;
            //Invoke("SetGhost", 0.5f);
        }
    }

    private void SetGhost()
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position
                      + (transform.right * _moveSpeed * Time.deltaTime * _horizontal)
                      + (transform.up * _moveSpeed * Time.deltaTime * _vertical);


        _rb.MovePosition(pos);
    }
}