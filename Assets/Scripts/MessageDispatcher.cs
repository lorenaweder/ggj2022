using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher : MonoBehaviour
{
    public static event Action<bool> OnAliveStateChanged;
    public static void NotifyAlive(bool isAlive) => OnAliveStateChanged?.Invoke(isAlive);

    private void OnDestroy()
    {
        OnAliveStateChanged = null;
    }
}
