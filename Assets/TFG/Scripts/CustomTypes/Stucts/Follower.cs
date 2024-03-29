using System;
using UnityEngine;

public struct Follower <T>
{
    [SerializeField] private int priority;
    [SerializeField] private T target;
    [SerializeField] private GameObject followerObject;

    public Follower (T initialTarget, int initPriority, GameObject initFollowerObject)
    {
        target = initialTarget;
        priority = initPriority;
        followerObject = initFollowerObject;
    }

    public T Target => target;
    public int Priority => priority;
    public GameObject FollowerObject => followerObject;


}
