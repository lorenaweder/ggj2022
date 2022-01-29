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

            // This is actually a bug and ideally we'd make the behavior with the correct set up,
            // but we can also just leave it, jam rules = chaos!!!!
            // the problem with this being a bug is that it collides with the ceiling or walls and loses up movement
            // because it was going on inertia alone
            _rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            _rb.isKinematic = false;

            gameObject.layer = _deadLayer;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position
                      + (transform.right * _moveSpeed * Time.deltaTime * _horizontal)
                      + (transform.up * _moveSpeed * Time.deltaTime * _vertical);


        _rb.MovePosition(pos);
    }
}