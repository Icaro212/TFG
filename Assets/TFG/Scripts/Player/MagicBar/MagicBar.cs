using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public const int maxPoints = 100;
    public int currentPoints;
    public float deactivateSeconds;
    private Image barImage;
    private Coroutine deactivateCoroutune;
    private float timer;

    private void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
        currentPoints = maxPoints;
        gameObject.SetActive(false);
    }

    public void Cost(float value)
    {
        currentPoints =  currentPoints - (int) value;
        float fillPercentaje =  (float) currentPoints / maxPoints;
        barImage.fillAmount = fillPercentaje;
        
        if(deactivateCoroutune != null)
        {
            timer = deactivateSeconds;
        }

        if(!gameObject.activeSelf && currentPoints < maxPoints)
        {
            gameObject.SetActive(true);
            timer = deactivateSeconds;
            deactivateCoroutune = StartCoroutine(DeactivateAfterDelay());
        }
        else if(currentPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        while(timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public bool CheckValidityMovement(float value)
    {
        return currentPoints >= value;
    }

    public void Reset()
    {
        currentPoints = maxPoints;
        barImage.fillAmount = 1;
        gameObject.SetActive(false);
    }
}
