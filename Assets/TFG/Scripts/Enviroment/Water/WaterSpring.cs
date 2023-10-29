using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterSpring : MonoBehaviour
{

    private int waveIndex = 0;
   
    [SerializeField]
    public SpriteShapeController spriteShapeControler = null;
    
    [System.NonSerialized]
    public float velocity = 0;
    private float resistance = 40f;
    public float force = 0;

    [System.NonSerialized]
    public float height = 0f; //Current height the object has
    private float target_height =  0f; // Height we outhg to get
    public Transform waterSplash;
    public float velocityLimit;


    public void Init(SpriteShapeController controller)
    {
        var index = transform.GetSiblingIndex();
        waveIndex = index + 1;
        spriteShapeControler = controller;

        velocity = 0;
        height = transform.localPosition.y;
        target_height = transform.localPosition.y;
    }


    public void WavePointUpdate()
    {
        if(spriteShapeControler != null)
        {
            Spline waterSline = spriteShapeControler.spline;
            Vector3 wavePosition = waterSline.GetPosition(waveIndex);
            waterSline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    public void WaveSpringUpdate(float springStiffness, float dampening, SpriteShapeController controller)
    {
        spriteShapeControler = controller;
        height = transform.localPosition.y;
        var x = height - target_height;
        var loss = -dampening * velocity;
        force = -springStiffness * x + loss;
        velocity += force;
        var y = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, y + velocity, transform.localPosition.z);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            var speed = rb.velocity;
            velocity += speed.y / resistance;
            if(-rb.velocity.y >= velocityLimit)
            {
                Transform newSplash = Instantiate(waterSplash, collision.transform.position, collision.transform.rotation);
                Destroy(newSplash.gameObject, 0.5f);
            }

        }
    }


    public string GetController()
    {
        return spriteShapeControler.name;
    }
}
