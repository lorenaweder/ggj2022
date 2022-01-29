using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private GameObject _lifeIcon;
    private List<GameObject> _lives;

    private void Awake()
    {
        MessageDispatcher.OnLivesChanged += OnLivesChanged; ;
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnLivesChanged -= OnLivesChanged;
    }

    private bool _firstTime = true;

    private void OnLivesChanged(int lives, int max)
    {
        if (_firstTime)
        {
            _lives = new List<GameObject>(max);
            for (int i = 0; i < max; i++)
            {
                var g = Instantiate(_lifeIcon, this.transform);
                g.SetActive(i < lives);
                _lives.Add(g);
            }
            _firstTime = false;
        }
        else
        {
            for (int i = 0; i < _lives.Count; i++)
            {
                _lives[i].gameObject.SetActive(i < lives);
            }
        }
    }
}
