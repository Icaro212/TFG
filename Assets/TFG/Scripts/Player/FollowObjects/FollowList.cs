using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowList : MonoBehaviour
{

    private Transform position;
    public List<GameObject> followers = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
    }

    public GameObject AddFollower()
    {
        GameObject newFollower = new GameObject("Follower");
        newFollower.transform.parent = gameObject.transform;
        newFollower.transform.position = new Vector3(position.position.x + 1.5f *followers.Count, position.position.y, position.position.z);
        followers.Add(newFollower);
        return newFollower;

    }

    public void RemoveTarget(GameObject objective)
    {
        followers.Remove(objective);
    }
    
}
