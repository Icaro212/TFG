using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingArea : MonoBehaviour
{

    private bool PlayerIsInArea=false; 


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerIsInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerIsInArea = false;
        }
    }

    public bool getPlayerIsInArea()
    {
        return PlayerIsInArea;
    }
}
