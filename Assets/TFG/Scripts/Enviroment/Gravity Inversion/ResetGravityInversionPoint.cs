using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGravityInversionPoint : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerData data;
    [SerializeField] private Vector2 orientationObj = new Vector2 (1,0);


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerTransform = other.GetComponent<Transform>();
            data = other.GetComponent<PlayerMovement>().data;
            playerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            data.JumpPositive = true;
            data.floorInXAixs = true;
            if ((Vector2.Dot(data.orientation, orientationObj) == -1) || (Vector2.SignedAngle(data.orientation, orientationObj) == -90 && data.orientation.y == 0) || (data.orientation.y != 0 && Vector2.SignedAngle(data.orientation, orientationObj) == 90))
            {
                playerTransform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
    }

   
}
