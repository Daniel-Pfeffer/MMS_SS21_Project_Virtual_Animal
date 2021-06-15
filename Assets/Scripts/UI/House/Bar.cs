using System;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image barEntity;
    [SerializeField] private Text description;

    public void setDescription(String text)
    {
        description.text = text;
    }

    public void UpdateBar(float value)
    {
        barEntity.fillAmount = value / 100;
    }

    public void UpdateBar(double value)
    {
        barEntity.fillAmount = (float) (value / 100);
    }
}