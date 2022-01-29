using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    [SerializeField] private int _initialLives;
    [SerializeField] private int _maxLives;

    private int _currentLives;

    private void Awake()
    {
        _currentLives = _initialLives;
    }

    private void Start()
    {
        MessageDispatcher.NotifyLives(_currentLives, _maxLives);
    }

    public bool LoseOne()
    {
        if(_currentLives > 0) _currentLives--;
        MessageDispatcher.NotifyLives(_currentLives, _maxLives);
        return _currentLives == 0;
    }

    public void GainOne()
    {
        _currentLives++;
        _currentLives = Mathf.Min(_maxLives, _currentLives);
        MessageDispatcher.NotifyLives(_currentLives, _maxLives);
    }
}
