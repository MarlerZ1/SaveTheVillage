using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float MaxTime;
    public bool Tick;

    private Image _img;
    private float _currentTime;

    private void Start() 
    {
        _img = GetComponent<Image>();
        _currentTime = MaxTime;
    }

    private void Update()
    {
        Tick = false;
        _currentTime -= Time.deltaTime;

        if (_currentTime < 0 )
        {
            _currentTime = MaxTime;
            Tick = true;
        }

        _img.fillAmount = _currentTime / MaxTime;

    }
}
