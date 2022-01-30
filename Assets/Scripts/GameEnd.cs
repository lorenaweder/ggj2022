using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private int _endResultsScreen;

    private void Awake()
    {
        MessageDispatcher.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        // potentially do a fade or something
        StartCoroutine(LoadNext());

        IEnumerator LoadNext()
        {
            yield return new WaitForSeconds(_delay);
            SoundManager.Instance.PlayEffect("CatDiedMouseLaughing");
            SceneManager.LoadScene(_endResultsScreen);
        }
    }
}
