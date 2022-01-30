using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Mouse _mousePrefab;
    [SerializeField] private float _cooldownAfterMouseKilled = 1f;
    [SerializeField] private float _minMiceSpeed = 1f;
    [SerializeField] private float _maxMiceSpeed = 4f;
    [SerializeField] private float _checkCatRadius;
    [SerializeField] private LayerMask _checkCatLayerMask;
    private Collider[] _hits = new Collider[2];

    private float _cooldownTime;
    private bool _canSpawn = true;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _checkCatRadius);
    }

    private void Start()
    {
        TryToSpawn();
    }

    private void Update()
    {
        TryToSpawn();
    }

    void TryToSpawn()
    {
        if (!_canSpawn) return;
        if (_cooldownTime > 0f)
        {
            _cooldownTime -= Time.deltaTime;
            return;
        }

        if (Physics.OverlapSphereNonAlloc(transform.position, _checkCatRadius, _hits, _checkCatLayerMask) > 0)
        {
            return;
        }

        var mouse = Instantiate(_mousePrefab, null).GetComponent<Mouse>();
        mouse.transform.position = transform.position;
        mouse.MoveSpeed = Random.Range(_minMiceSpeed, _maxMiceSpeed);
        mouse.Spawner = this;

        _canSpawn = false;
    }

    public void ReportMouseDead()
    {
        _canSpawn = true;
        // So we can't camp
        _cooldownTime = _cooldownAfterMouseKilled;
    }
}
