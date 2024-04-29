using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfSearch : MonoBehaviour
{

    private Collider2D col;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public List<Collider2D> SearchAllKeys()
    {
        Collider2D[] hitColliders = new Collider2D[1000];
        ContactFilter2D contactFilter = new ContactFilter2D();
        int colliderCount = Physics2D.OverlapCollider(col, contactFilter.NoFilter(), hitColliders);
        List<Collider2D> listOfKeys = new List<Collider2D>();
        for (int i = 0; i < colliderCount; i++)
        {
            if (i >= hitColliders.Length)
            {
                break;
            }

            GameObject elemnt = hitColliders[i].gameObject;
            if (elemnt.CompareTag("Key"))
            {
                listOfKeys.Add(elemnt.GetComponent<Collider2D>());
            }
        }
        return listOfKeys;
    }
}
