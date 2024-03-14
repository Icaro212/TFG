using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectWallHori : MonoBehaviour
{

    public bool isInArea { set; get; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInArea = false;
        }
    }
}
