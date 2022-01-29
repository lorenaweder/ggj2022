using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSchemeUI : MonoBehaviour
{
    [SerializeField] private GameObject _alive;
    [SerializeField] private GameObject _dead;

    private void Awake()
    {
        MessageDispatcher.OnAliveStateChanged += OnAliveStateChanged; ;
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnAliveStateChanged -= OnAliveStateChanged;
    }

    private void OnAliveStateChanged(bool isAlive)
    {
        _alive.SetActive(isAlive);
        _dead.SetActive(!isAlive);
    }
}
