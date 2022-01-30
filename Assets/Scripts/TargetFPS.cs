using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{
    [SerializeField] private int _fps = 60;

    void Start()
    {
        Application.targetFrameRate = _fps;
    }
}
