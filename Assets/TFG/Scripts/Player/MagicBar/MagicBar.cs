using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public const int maxPoints = 100;
    public int currentPoints;
    private Image barImage;

    private void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
        currentPoints = maxPoints;
    }

    public void Cost(float value)
    {
        currentPoints =  currentPoints - (int) value;
        float fillPercentaje =  (float) currentPoints / maxPoints;
        barImage.fillAmount = fillPercentaje;
    }

    public bool CheckValidityMovement(float value)
    {
        return currentPoints >= value;
    }

    public void Reset()
    {
        currentPoints = maxPoints;
        barImage.fillAmount = 1;
    }
}
