using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    [SerializeField] private LayerMask _aliveView;
    [SerializeField] private LayerMask _deadView;

    Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        MessageDispatcher.OnAliveStateChanged += OnCatAliveStateChanged;
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnAliveStateChanged -= OnCatAliveStateChanged;
    }

    private void OnCatAliveStateChanged(bool isAlive)
    {
        _camera.cullingMask = isAlive ? _aliveView : _deadView;
    }
}
