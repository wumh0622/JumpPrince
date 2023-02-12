using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Monster : MonoBehaviour
{
    

    private float dashingTime = 0.2f;
    private float targetY = 0f;

    private AudioSource MonsterAudio;

    [SerializeField] public AudioClip scareAudio;
    [SerializeField] private Animation moveShake;
    [SerializeField] private float movingPower = 12f;
    [SerializeField] private float moveDistance = 1f;


    private void Awake()
    {
        MonsterAudio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetY), movingPower * Time.deltaTime);
    }

    public void ResetPosition()
    {
        targetY = 0f;
        transform.position = new Vector2(0f, 0f);
    }
    public void MonsterMoveUp()
    {
        targetY = transform.position.y + moveDistance;
        moveShake.Play();
    }

    void MonsterScare()
    {
        if (MonsterAudio.clip)
        {
            MonsterAudio.Play();
        }
    }
}
