using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Monster : MonoBehaviour
{
    private float dashingTime = 0.2f;
    private float targetY = 0f;
    private float originTargetY = 0f;
    private Vector3 originPosition;
    private float moveTime = 0f;

    private AudioSource MonsterAudio;
    private GameObject player;
    private bool onAtk = false;
    private bool playerDie = false;

    [SerializeField] public AudioClip scareAudio;
    [SerializeField] public AudioClip showupAudio;
    [SerializeField] private Animator animControl;
    [SerializeField] private float maxDistanceToPlayer = 12f;
    [SerializeField] private float movingPower = 12f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float atkRange = 7f;
    [SerializeField] private float atkDetectionRange = 12f;
    [SerializeField] private float moveInterval = 1f;


    private void Awake()
    {
        //MonsterAudio = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        originTargetY = transform.position.y;
        targetY = originTargetY;
        originPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), movingPower * Time.deltaTime);
        if (!onAtk && !playerDie)
        {
            if (Time.time > moveTime)
            {
                moveTime += moveInterval;
                MonsterMoveUp();
            }
            CheckPlayerOnRange();
        }
    }

    public void ResetPositionAndState()
    {
        targetY = originTargetY;
        transform.position = originPosition;
        playerDie = false;
    }
    public void MonsterMoveUp()
    {
        targetY = transform.position.y + moveDistance;
        //moveShake.Play("moveUp");
        //MonsterScare(scareAudio);
    }

    public void MonsterShowUp()
    {
        targetY = transform.position.y + 2.3842f;
        //moveShake.Play("showup");
        //MonsterScare(showupAudio);
    }

    public void MonsterAtk()
    {
        onAtk = true;
        animControl.SetTrigger("Atk");
        //targetY = transform.position.y + 2.3842f;
        //moveShake.Play("showup");
        //MonsterScare(showupAudio);
        SimpleTimerManager.instance.RunTimer(CheckPlayerBeAttacked, 0.6f);
        SimpleTimerManager.instance.RunTimer(restAtk, 1f);
    }

    private void restAtk()
    {
        onAtk = false;
    }

    private void CheckPlayerOnRange()
    {
        if(player.transform.position.y - transform.position.y < atkDetectionRange)
        {
            MonsterAtk();
        }
    }

    private void CheckPlayerBeAttacked()
    {
        if (player.transform.position.y - transform.position.y < atkRange)
        {
            playerDie = true;
            //player die
        }
    }


    void MonsterScare(AudioClip clip)
    {
        MonsterAudio.clip = clip;
        if (MonsterAudio.clip)
        {
            MonsterAudio.Play();
        }
    }
}
