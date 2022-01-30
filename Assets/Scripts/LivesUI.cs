using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LivesUI : MonoBehaviour
{
    private List<GameObject> _missing;

    private void Awake()
    {
        _missing = new List<GameObject>(9);

        for (int i = 0; i < transform.childCount; i++)
        {
            // Ugly fix to not hand place all the lives again
            var l = transform.GetChild(i);
            _missing.Add(l.GetChild(1).gameObject);
        }

        MessageDispatcher.OnLivesChanged += OnLivesChanged;
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnLivesChanged -= OnLivesChanged;
    }

    private bool _firstTime = true;

    private void OnLivesChanged(int lives, int max)
    {
        Assert.IsTrue(max >= lives);
        Assert.IsTrue(max <= _missing.Count);
        for (int i = 0; i < max; i++)
        {
            _missing[i].SetActive(i >= lives);
        }
    }
}
