using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectY : MonoBehaviour
{
    private Transform selfTransform;

    public GameObject obj;
    private Transform objTransform;

    void Awake()
    {
        selfTransform = GetComponent<Transform>();
        objTransform = obj.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        selfTransform.position = new Vector3(selfTransform.position.x, objTransform.position.y, selfTransform.position.z); 
    }
}
