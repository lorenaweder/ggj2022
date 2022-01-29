using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTrigger : MonoBehaviour
{
    [SerializeField] private Cat _cat;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Mouse>(out var mouse)) return;
        mouse.HitByCat();
        _cat.HitMouse();
    }
}
