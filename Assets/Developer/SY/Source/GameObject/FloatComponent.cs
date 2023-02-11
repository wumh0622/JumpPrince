using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatComponent : MonoBehaviour
{
    public float Range = 5;

    protected Transform transform;

    protected float time;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Awake()
    {
        transform = GetComponent<Transform>();
    }
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        transform.position = initialPosition + new Vector3(Mathf.Sin(time) * Range, 0, 0);
    }
}
