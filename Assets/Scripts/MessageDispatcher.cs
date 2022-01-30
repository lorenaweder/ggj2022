using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher
{
    public static event Action<int> OnMouseKilled;

    public static event Action<bool> OnAliveStateChanged;
    public static void NotifyAlive(bool isAlive) => OnAliveStateChanged?.Invoke(isAlive);

    public static event Action OnGameOver;
    public static void NotifyGameOver() => OnGameOver?.Invoke();

    public static event Action<int, int> OnLivesChanged;
    public static void NotifyLives(int lives, int max) => OnLivesChanged?.Invoke(lives, max);

    private static int _score;
    public static int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnMouseKilled?.Invoke(value);
        }
    }
}
