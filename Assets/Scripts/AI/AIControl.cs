using System.Collections;
using Unity.VisualScripting;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.AI;

public class AIControl : MonoBehaviourPun
{
    public GameObject[] players;
    private GameObject closerPlayer;

    private bool isSantiActive = false;
    private bool isJoseActive = false;

    public NavMeshAgent aiAgent;               //  Nav mesh agent component
    static float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 1;                  //  Wait time when the enemy detect near the player without seeing
    public float walkSpeed = 6;                     //  Walking speed, speed in the nav mesh agent
    public float chaseSpeed = 9;                      //  Running speed

    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something


    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    public float WaitTime;                               //  Variable of the wait time that makes the delay
    float minWaitTime;
    float maxWiatTime;
    float TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    public bool playerInRange;                           //  If the player is in range of vision, state of chasing
    //public bool isPlayerNear;                              //  If the player is near, state of hearing
    public bool isPatrol;                                //  If the enemy is patrol, state of patroling
    public bool isPlayerCaught;                            //  if the enemy has caught the player
    public bool isChasing;
    private bool santiDowned;
    private bool joseDowned;

    [SerializeField]
    private bool blinkingSanti;
    [SerializeField]
    private bool blinkingJose;
    private PhotonView JosePV;
    private PhotonView SantiPV;



    //Testing variables
    public bool isSeen;  //Pascualita is being seen by player
    public bool isSeenSanti = false;  //Pascualita is being seen by player
    public bool isSeenJose = false;  //Pascualita is being seen by player
    public bool isSeenBoth = false;

    //int randNum; //Random val to randomize patrol pattern
    public float catchDistance;
    public Animator aiAnimation; //for fuuture use in animations
    public Animator santiAnimation; //change for santi's animation in other script (temporarily here)
    public Animator joseAnimation; //change for santi's animation in other script (temporarily here)


    public float seenCooldownTimer;
    public float stoppedTimer;
    public float defaultCooldownTime = 5f;

    void Start()
    {
        players = new GameObject[2];

        PlayerPosition = Vector3.zero;
        isPatrol = true;
        isPlayerCaught = false;


        //Testing

        isSeen = false;
        isChasing = false;
        //aiAgent.destination = waypoints[CurrentWaypointIndex].position;
        //randNum = 0;
        minWaitTime = 1f;
        maxWiatTime = 3f;
        catchDistance = 2f;

        stoppedTimer = defaultCooldownTime;
        seenCooldownTimer = defaultCooldownTime;
        //Testing

        WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        TimeToRotate = timeToRotate;

        CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        aiAgent = GetComponent<NavMeshAgent>();
        aiAgent.acceleration = 30f;
        aiAgent.isStopped = false;
        aiAgent.speed = walkSpeed;             //  Set the navemesh speed with the normal speed of the enemy
        aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);    //  Set the destination to the first waypoint
    }

    [PunRPC]
    public void BlinkRPC(string name, bool isBlinking)
    {
        if (name == "Jose(Clone)")
        {
            blinkingJose = isBlinking;
        }
        else
        {
            blinkingSanti = isBlinking;
        }
    }

    [PunRPC]
    public void SyncInRange(bool range)
    {

    }



    private void Update()
    {
        if (isJoseActive && isSantiActive)
        {
            EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

            closerPlayer = GetCloserPlayer(); //relevant player

            isSeenBoth = isSeenJose && isSeenSanti;

            if (isSeenBoth && !blinkingSanti && !blinkingJose)
            {
                // Both players are seen and not blinking
                aiAnimation.SetBool("stop", true);
                Stop();
            }
            // Either only Santi or only Jose is seen
            else if (!isSeenBoth && isSeenSanti && !blinkingSanti)
            {
                aiAnimation.SetBool("stop", true);
                Stop();
            }
            else if (!isSeenBoth && isSeenJose && !blinkingJose)
            {
                // Only Jose is seen and not blinking
                aiAnimation.SetBool("stop", true);
                Stop();
            }
            else if (isChasing && !isPlayerCaught)
            {
                aiAnimation.ResetTrigger("attack");
                aiAnimation.SetBool("stop", false);
                aiAnimation.SetTrigger("pose1");
                Chasing();
            }
            else if (isPatrol && !isPlayerCaught)
            {
                aiAnimation.ResetTrigger("attack");
                aiAnimation.SetBool("stop", false);
                aiAnimation.SetTrigger("pose1");
                Patroling();
            }
        }
    }

    private void Chasing()
    {

        //  The enemy is chasing the player
        //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!isPlayerCaught)
        {
            Move(chaseSpeed);
            aiAgent.SetDestination(PlayerPosition);          //  set the destination of the enemy to the player location
        }
        if (aiAgent.remainingDistance <= aiAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (WaitTime <= 0 && !isPlayerCaught && Vector3.Distance(transform.position, closerPlayer.transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                isPatrol = true;
                isChasing = false;
                Move(walkSpeed);
                TimeToRotate = timeToRotate;
                WaitTime = startWaitTime;
                aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, closerPlayer.transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                WaitTime -= Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, closerPlayer.transform.position) < catchDistance)
            {
                CaughtPlayer();
                Attacking();
            }
        }
    }
    public void santiActivation()
    {
        players[0] = GameObject.FindGameObjectWithTag("PlayerSanti");
        isSantiActive = true;
        SantiPV = players[0].GetComponent<PhotonView>();
        santiAnimation = players[0].GetComponent<Animator>();
        //santiCamera = GameObject.Find("JumpscareCamera").GetComponent<Camera>();
    }

    public void joseActivation()
    {
        players[1] = GameObject.FindGameObjectWithTag("PlayerJose");
        isJoseActive = true;
        JosePV = players[1].GetComponent<PhotonView>();
        joseAnimation = players[1].GetComponent<Animator>();
    }

    private void Patroling()
    {
        //  The player is no near when the enemy is platroling
        playerLastPosition = Vector3.zero;
        Move(walkSpeed);
        aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
        if (aiAgent.remainingDistance <= aiAgent.stoppingDistance)
        {
            //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
            if (WaitTime <= 0)
            {
                NextPoint();
                Move(walkSpeed);
                WaitTime = Random.Range(minWaitTime, maxWiatTime);
            }
            else
            {
                Stop();
                WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Attacking()
    {

        if (closerPlayer == players[0])
        {
            if (!SantiPV.IsMine)
            {
                isPlayerCaught = false;
                return;
            }
            isPlayerCaught = false;
            Debug.Log("Pinga attack SANTI");
            SantiPV.RPC("SyncDowned", RpcTarget.All);
            GameManager.Instance.PascualitaJumpscare.Play();
            SantiPV.RPC("UpdateJumpScareAnimationSanti", RpcTarget.All);
            aiAnimation.SetBool("stop", false);
            aiAnimation.ResetTrigger("pose1");
            aiAnimation.SetTrigger("attack");
            StartCoroutine(EndSantiJumpscare());


        }
        else if (closerPlayer == players[1])
        {
            if (!JosePV.IsMine)
            {
                isPlayerCaught = false;
                return;
            }

            isPlayerCaught = false;
            Debug.Log("Pinga attack JOSE");
            JosePV.RPC("SyncDowned", RpcTarget.All);
            GameManager.Instance.PascualitaJumpscare.Play();
            JosePV.RPC("UpdateJumpScareAnimationJose", RpcTarget.All);
            aiAnimation.SetBool("stop", false);
            aiAnimation.ResetTrigger("pose1");
            aiAnimation.SetTrigger("attack");
            StartCoroutine(EndJoseJumpscare());
        }


        Patroling();

    }
    private void Seen()
    {
        Stop();
        //StartCoroutine(IsSeenTimer());
        //if (stoppedTimer >= 0) // pascualita is stopped (cambiar wait time)
        //{
        //    Stop();
        //    stoppedTimer -= Time.deltaTime;
        //    seenCooldownTimer = defaultCooldownTime;
        //}
        //else //is on cooldown from being seen
        //{   
        //    if(seenCooldownTimer >= 0) //
        //    {
        //        Debug.Log("Player is controlled");
        //        seenCooldownTimer -= Time.deltaTime;
        //        Move(walkSpeed);
        //        //isPatrol = true;
        //        //aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
        //    }
        //    else
        //    {
        //        Debug.Log("Undo ivnecible pascuala");
        //        stoppedTimer = defaultCooldownTime;
        //        //TimeToRotate = timeToRotate;  
        //    }
        //}
    }

    private void OnAnimatorMove()
    {

    }

    public void NextPoint()
    {
        CurrentWaypointIndex = Random.Range(0, waypoints.Length);
        aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
    }

    void Stop()
    {
        aiAgent.isStopped = true;
        aiAgent.speed = 0;
    }

    void Move(float speed)
    {
        aiAgent.isStopped = false;
        aiAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        isPlayerCaught = true;
    }

    void LookingPlayer(Vector3 player)
    {
        aiAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (WaitTime <= 0)
            {

                Move(walkSpeed);
                aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
                WaitTime = startWaitTime;
                TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                WaitTime -= Time.deltaTime;
            }
        }
    }

    private GameObject GetCloserPlayer()
    {
        GameObject close;
        float distanceToSanti;
        float distanceToJose;
        if (players[0] != null && players[1] != null)
        {
            distanceToSanti = Vector3.Distance(transform.position, players[0].transform.position);
            distanceToJose = Vector3.Distance(transform.position, players[1].transform.position);
            close = (distanceToSanti < distanceToJose) ? players[0] : players[1];
        }
        else if (players[1] != null)
        {
            close = players[1];
        }
        else if (players[0] != null)
        {
            close = players[0];
        }
        else
        {
            close = null;
        }

        return close;
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            if (player.GetComponent<Down>().isPlayerDowned)
            {
                isChasing = false;
                isPatrol = true;
                break;
            }
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask) && !isPlayerCaught)
                {
                    this.playerInRange = true;             //  The player has been seen by the enemy and then the enemy chases the player
                    isChasing = true;                 //  Change the state to chasing the player
                    isPatrol = false;

                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    this.playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                this.playerInRange = false;                //  Change the sate of chasing
            }
            if (this.playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }

    [PunRPC]
    public void SyncSetIsSeen(bool setIsSeen, string names)
    {
        if (names == "Jose(Clone)")
        {
            isSeenJose = setIsSeen;
        }
        else if (names == "Santi(Clone)")
        {
            isSeenSanti = setIsSeen;
        }
        isSeen = isSeenJose || isSeenSanti;
    }


    public bool GetIsSeen()
    { return isSeen; }

    IEnumerator EndSantiJumpscare()
    {

        yield return new WaitForSeconds(2f);

        aiAnimation.SetBool("stop", false);
        aiAnimation.ResetTrigger("attack");
        aiAnimation.SetTrigger("pose1");
        SantiPV.RPC("ResetJumpScareAnimationSanti", RpcTarget.All);
        
    }                                         

    IEnumerator EndJoseJumpscare()
    {

        yield return new WaitForSeconds(2f);

        aiAnimation.SetBool("stop", false);
        aiAnimation.ResetTrigger("attack");
        aiAnimation.SetTrigger("pose1");
        JosePV.RPC("ResetJumpScareAnimationSanti", RpcTarget.All);
        

    }



    

    

    
}