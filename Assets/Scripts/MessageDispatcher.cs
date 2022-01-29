using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher : MonoBehaviour
{
    public static event Action<bool> OnAliveStateChanged;
    public static void NotifyAlive(bool isAlive) => OnAliveStateChanged?.Invoke(isAlive);

    public static event Action OnGameOver;
    public static void NotifyGameOver() => OnGameOver?.Invoke();

    public static event Action<int> OnLivesChanged;
    public static void NotifyLives(int lives) => OnLivesChanged?.Invoke(lives);

    private void OnDestroy()
    {
        OnAliveStateChanged = null;
        OnLivesChanged = null;
        OnGameOver = null;
    }
}
