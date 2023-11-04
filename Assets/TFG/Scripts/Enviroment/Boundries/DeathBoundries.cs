using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBoundries : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.Die();
        }
    }
}
