using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        MessageDispatcher.OnMouseKilled += OnMouseKilled;
    }

    private void Destroy()
    {
        MessageDispatcher.OnMouseKilled -= OnMouseKilled;
    }

    private void OnMouseKilled(int count)
    {
        _text.SetText(count.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        _text.SetText(MessageDispatcher.Score.ToString());
    }
}
