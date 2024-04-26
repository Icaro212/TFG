using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInversionPoint : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private Transform playerTransform;
    private PlayerData data;
    [SerializeField] private Vector2 gravityDirection = Vector3.down;
    [SerializeField] private int degree;
    [SerializeField] private Vector2 orientationObj;
    [SerializeField] private AudioClip gravityChangeClip;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        data = player.GetComponent<PlayerMovement>().data;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundFXManager.instance.PlaySoundFXClip(gravityChangeClip, transform, 1f);
            ChangeGravityDirection();
            RotateControls();
        }
    }

    private void RotateControls()
    {
        playerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        switch (degree)
        {
        case 90:
                playerTransform.Rotate(0.0f, 0.0f, 270.0f);
                data.JumpPositive = true;
                data.floorInXAixs = false;
                break;
        case 180:
                playerTransform.Rotate(0.0f, 0.0f, 180.0f);
                data.JumpPositive = false;
                data.floorInXAixs = true;
                break;
        case 270:
                playerTransform.Rotate(0.0f, 0.0f, 90.0f);
                data.JumpPositive = false;
                data.floorInXAixs = false;

                break;
        default:
                data.JumpPositive = true;
                data.floorInXAixs = true;
                break;
        }

        //Character Alligment
        if((Vector2.Dot(data.orientation, orientationObj) == -1) || (Vector2.SignedAngle(data.orientation, orientationObj) == -90 && data.orientation.y == 0) || (data.orientation.y != 0 && Vector2.SignedAngle(data.orientation, orientationObj) == 90)) 
        {
            playerTransform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }
   

    private void ChangeGravityDirection()
    {
        Physics2D.gravity = gravityDirection;
    }
}
