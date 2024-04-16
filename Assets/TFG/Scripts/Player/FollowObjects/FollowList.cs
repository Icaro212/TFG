using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowList : MonoBehaviour
{

    private Transform position;
    private Transform playerTransform;
    public List<GameObject> followers = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
        playerTransform = this.transform.parent;
    }

    public GameObject AddFollower()
    {
        GameObject newFollower = new GameObject("Follower");
        newFollower.transform.parent = gameObject.transform;
        newFollower.transform.position = new Vector3(position.position.x + 1.5f * followers.Count * -playerTransform.forward.z, position.position.y, position.position.z);
        followers.Add(newFollower);
        return newFollower;

    }

    public void UpdateTargetsPosition()
    {
        for(int i = 0; i<followers.Count; i++)
        {
            GameObject follower = followers[i];
            follower.transform.position = new Vector3(position.position.x + 1.5f * i * -playerTransform.forward.z, position.position.y, position.position.z);
        }
    }

    public void RemoveTarget(GameObject objective)
    {
        followers.RemoveAt(followers.IndexOf(objective));
        Destroy(objective);
    }
    
}
