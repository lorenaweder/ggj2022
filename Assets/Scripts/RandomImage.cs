using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private int _change = 15;
    private int _index;

    private void Update()
    {
        if (Time.frameCount % _change == 0)
        {
            _index = (_index + 1) % _sprites.Count;
            _image.sprite = _sprites[_index];
        }
    }
}
