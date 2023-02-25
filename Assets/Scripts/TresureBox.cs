using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TresureBox : MonoBehaviour
{
    public UnityEvent OnTouchBox;

    Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collider.enabled = false;
        OnTouchBox.Invoke();
    }
}
