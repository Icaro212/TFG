using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCharge : MonoBehaviour
{

    public int habilityRegen;
    public GameObject canvas;
    private MagicBar bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bar.Cost(-habilityRegen);
        Destroy(this.gameObject);
    }

}
