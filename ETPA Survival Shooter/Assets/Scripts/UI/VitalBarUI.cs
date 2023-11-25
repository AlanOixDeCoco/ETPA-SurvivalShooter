using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalBarUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    private void Start()
    {
        _fillImage.fillAmount = 1f;
    }

    public void UpdateBar(float value, float maxValue)
    {
        float fillAmout = value / maxValue;
        _fillImage.fillAmount = fillAmout;
    }
}
